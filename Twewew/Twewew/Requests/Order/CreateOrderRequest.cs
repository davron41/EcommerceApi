using Twewew.Enums;

namespace Twewew.Requests.Order;

public record CreateOrderRequest(
    DateTime OrderDate,
    decimal TotalQuantity,
    OrderStatus OrderStatus);
