namespace Twewew.Requests.OrderItem;

public record CreateOrderItemRequest(
    int Quantity,
    decimal UnitPrice,
    int OrderId,
    int ProductId);
