using Twewew.Enums;

namespace Twewew.DTOs;

public record OrderDto(
    int Id,
    DateTime OrderDate,
    decimal TotalQuantity,
    OrderStatus OrderStatus);
