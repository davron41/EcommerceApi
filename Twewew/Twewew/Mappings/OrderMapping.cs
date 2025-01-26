using AutoMapper;
using Twewew.DTOs;
using Twewew.Entities;
using Twewew.Requests.Order;

namespace Twewew.Mappings;

public class OrderMapping : Profile
{
    public OrderMapping()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<CreateOrderRequest, Order>();
        CreateMap<UpdateOrderRequest, Order>();
    }

}
