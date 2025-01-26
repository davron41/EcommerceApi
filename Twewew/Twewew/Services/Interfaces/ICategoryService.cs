using Twewew.DTOs;
using Twewew.Requests.Category;

namespace Twewew.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAsync();
    Task<CategoryDto> GetByIdAsync(CategoryRequest request);
    Task<List<ProductDto>> GetByCategoryAsync(CategoryRequest request);
    Task<CategoryDto> CreateAsync(CreateCategoryRequest request);
    Task UpdateAsync(UpdateCategoryRequest request);
    Task DeleteAsync(CategoryRequest request);
}
