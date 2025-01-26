namespace Twewew.Requests.Product;

public record CreateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    string[]? ImageUrl,
    DateTime AddedDate,
    bool IsFrozen,
    int CategoryId);
