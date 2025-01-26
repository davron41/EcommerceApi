namespace Twewew.Requests.Category;

public record UpdateCategoryRequest(
    int Id,
    string Name,
    string Description)
    : CreateCategoryRequest(
        Name,
        Description);
