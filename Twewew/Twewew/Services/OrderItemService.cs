using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Twewew.DTOs;
using Twewew.Entities;
using Twewew.Exceptions;
using Twewew.Persistence;
using Twewew.Requests.OrderItem;
using Twewew.Services.Interfaces;

namespace Twewew.Services;

public sealed class OrderItemService : IOrderItemService
{
    private readonly ICurrentUserService _userService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public OrderItemService(
        ICurrentUserService userService,
        ApplicationDbContext context,
        IMapper mapper)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<OrderItemDto>> GetAsync()
    {
        var userId = _userService.GetUserId();
        var items = _context.Items
              .AsNoTracking()
              .Where(x => x.Order.CustomerId == userId);

        var dto = await items
            .ProjectTo<OrderItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return dto;
    }
    public async Task<OrderItemDto> GetByIdAsync(OrderItemRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await GetAndValidateItems(request.Id);

        var dto = _mapper.Map<OrderItemDto>(item);

        return dto;
    }
    public async Task<OrderItemDto> CreateAsync(CreateOrderItemRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = _mapper.Map<OrderItem>(request);

        _context.Items.Add(item);
        await _context.SaveChangesAsync();
        var dto = _mapper.Map<OrderItemDto>(item);

        return dto;

    }
    public async Task UpdateAsync(UpdateOrderItemRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await GetAndValidateItems(request.Id);
        _mapper.Map(request, item);
        _context.Items.Update(item);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(OrderItemRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await GetAndValidateItems(request.Id);

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
    }

    private Task<OrderItem> GetAndValidateItems(int itemId)
    {
        var userId = _userService.GetUserId();
        var entity = _context.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Order.CustomerId == userId && x.Id == itemId);

        if (entity is null)
        {
            throw new EntityNotFoundException($"OrderItem with id:{itemId} is not found!");
        }
        return entity;
    }

}
