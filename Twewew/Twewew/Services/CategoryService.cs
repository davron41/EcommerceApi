using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Twewew.DTOs;
using Twewew.Entities;
using Twewew.Exceptions;
using Twewew.Persistence;
using Twewew.Requests.Category;
using Twewew.Services.Interfaces;

namespace Twewew.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CategoryService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<List<CategoryDto>> GetAsync()
    {

        var query = _context.Categories
            .AsNoTracking();

        var categories = await query
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return categories;
    }
    public async Task<CategoryDto> GetByIdAsync(CategoryRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var category = await GetAndValidateCategory(request.Id);

        var dto = _mapper.Map<CategoryDto>(category);
        return dto;
    }
    public async Task<List<ProductDto>> GetByCategoryAsync(CategoryRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var products = await _context.Products
            .Where(x => x.CategoryId == request.Id)
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return products;
    }
    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var category = _mapper.Map<Category>(request);

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<CategoryDto>(category);

        return dto;
    }
    public async Task UpdateAsync(UpdateCategoryRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var category = await GetAndValidateCategory(request.Id);

        _mapper.Map(request, category);

        _context.Categories.Update(category);
        await _context.SaveChangesAsync();

    }
    public async Task DeleteAsync(CategoryRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var category = await GetAndValidateCategory(request.Id);

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }


    private async Task<Category> GetAndValidateCategory(int categoryId)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == categoryId);

        if (category is null)
        {
            throw new EntityNotFoundException($"Category with id:{categoryId} is not found!");
        }
        return category;
    }


}
