namespace Twewew.Requests.OrderItem;

public record UpdateOrderItemRequest(int Id,
    int Quantity,
    decimal UnitPrice,
    int OrderId,
    int ProductId) :
    CreateOrderItemRequest(
        Quantity,
        UnitPrice,
        OrderId,
        ProductId);
