using AutoMapper;
using Twewew.DTOs;
using Twewew.Entities;
using Twewew.Requests.OrderItem;

namespace Twewew.Mappings;

public class OrderItemMapping : Profile
{
    public OrderItemMapping()
    {
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<CreateOrderItemRequest, OrderItem>();
        CreateMap<UpdateOrderItemRequest, OrderItem>();
    }
}
