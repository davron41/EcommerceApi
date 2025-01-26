using Twewew.DTOs;
using Twewew.Requests.Order;

namespace Twewew.Services.Interfaces;

public interface IOrderService
{
    Task<List<OrderDto>> GetAsync();
    Task<OrderDto> GetByIdAsync(OrderRequest request);
    Task<OrderDto> CreateAsync(CreateOrderRequest request);
    Task UpdateAsync(UpdateOrderRequest request);
    Task DeleteAsync(OrderRequest request);
}
