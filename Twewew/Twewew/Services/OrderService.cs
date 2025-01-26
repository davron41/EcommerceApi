using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Twewew.DTOs;
using Twewew.Entities;
using Twewew.Exceptions;
using Twewew.Persistence;
using Twewew.Requests.Order;
using Twewew.Services.Interfaces;

namespace Twewew.Services;

public sealed class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IMapper _mapper;

    public OrderService(ApplicationDbContext context, IMapper mapper, ICurrentUserService userService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    }
    public async Task<List<OrderDto>> GetAsync()
    {
        var userId = _userService.GetUserId();
        var query = _context.Orders
            .AsNoTracking()
            .Where(x => x.CustomerId == userId);

        var dto = await query
            .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return dto;
    }
    public async Task<OrderDto> GetByIdAsync(OrderRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var order = await GetAndValidateOrderAsync(request.Id);

        var dto = _mapper.Map<OrderDto>(order);

        return dto;
    }
    public async Task<OrderDto> CreateAsync(CreateOrderRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var order = _mapper.Map<Order>(request);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<OrderDto>(order);

        return dto;
    }
    public async Task UpdateAsync(UpdateOrderRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await GetAndValidateOrderAsync(request.Id);

        _mapper.Map(request, entity);

        _context.Orders.Update(entity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(OrderRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var order = await GetAndValidateOrderAsync(request.Id);

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    private async Task<Order> GetAndValidateOrderAsync(int orderId)
    {
        var userId = _userService?.GetUserId();
        var entity = await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CustomerId == userId && x.Id == orderId);

        if (entity is null)
        {
            throw new EntityNotFoundException($"Order with id:{orderId} is not found");
        }

        return entity;
    }
}

