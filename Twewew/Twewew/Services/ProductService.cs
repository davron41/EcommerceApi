using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Twewew.DTOs;
using Twewew.Entities;
using Twewew.Exceptions;
using Twewew.Persistence;
using Twewew.Requests.Product;
using Twewew.Services.Interfaces;

namespace Twewew.Services;

public sealed class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<ProductDto>> GetAsync()
    {
        var query = _context.Products
            .AsNoTracking();

        var dto = await query
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return dto;
    }
    public async Task<ProductDto> GetByIdAsync(ProductRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var category = await GetAndValidateProduct(request.Id);

        var dto = _mapper.Map<ProductDto>(category);

        return dto;
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = _mapper.Map<Product>(request);

        _context.Products.Add(entity);
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<ProductDto>(entity);

        return dto;
    }

    public async Task UpdateAsync(UpdateProductRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var product = await GetAndValidateProduct(request.Id);

        _mapper.Map(request, product);

        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProductRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var product = await GetAndValidateProduct(request.Id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    private Task<Product> GetAndValidateProduct(int productId)
    {
        var entity = _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == productId);

        if (entity is null)
        {
            throw new EntityNotFoundException($"Product with id {productId} is not found");
        }
        return entity;
    }

}
