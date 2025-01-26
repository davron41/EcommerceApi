using Twewew.Enums;

namespace Twewew.Requests.Order;

public record UpdateOrderRequest(
    int Id,
    DateTime OrderDate,
    decimal TotalQuantity,
    OrderStatus OrderStatus) :
    CreateOrderRequest(
      OrderDate,
      TotalQuantity,
      OrderStatus);
