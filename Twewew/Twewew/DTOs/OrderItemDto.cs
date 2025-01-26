namespace Twewew.DTOs;

public record OrderItemDto(
    int Id,
    int Quantity,
    decimal UnitPrice,
    int OrderId,
    int ProductId);
