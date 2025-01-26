using Twewew.DTOs;
using Twewew.Requests.Product;

namespace Twewew.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAsync();
    Task<ProductDto> GetByIdAsync(ProductRequest request);
    Task<ProductDto> CreateAsync(CreateProductRequest request);
    Task UpdateAsync(UpdateProductRequest request);
    Task DeleteAsync(ProductRequest request);
}
