namespace Twewew.DTOs;

public record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    byte[] ImagesUrl,
    DateTime AddedDate,
    bool IsFrozen,
    int CategoryId);
