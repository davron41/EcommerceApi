namespace Twewew.Requests.Product;

public record UpdateProductRequest(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    string[]? ImageUrl,
    DateTime AddedDate,
    bool IsFrozen,
    int CategoryId) :
    CreateProductRequest(
        Name,
        Description,
        Price,
        ImageUrl,
        AddedDate,
        IsFrozen,
        CategoryId);
