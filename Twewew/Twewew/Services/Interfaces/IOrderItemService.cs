using Twewew.DTOs;
using Twewew.Requests.OrderItem;

namespace Twewew.Services.Interfaces;

public interface IOrderItemService
{
    Task<List<OrderItemDto>> GetAsync();
    Task<OrderItemDto> GetByIdAsync(OrderItemRequest request);
    Task<OrderItemDto> CreateAsync(CreateOrderItemRequest request);
    Task UpdateAsync(UpdateOrderItemRequest request);
    Task DeleteAsync(OrderItemRequest request);
}
