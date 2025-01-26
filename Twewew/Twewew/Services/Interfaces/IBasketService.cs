using Twewew.Entities;
using Twewew.Requests.Product;

namespace Twewew.Services.Interfaces;

public interface IBasketService
{
    public Task<IEnumerable<Product>> GetProductsAsync();
    public Task<Product> GetProductByIdAsync(ProductRequest request);
    Task<bool> FreezeProductAsync(ProductRequest request, Guid userId);
    Task UnfreezeProductAsync(ProductRequest request);
}
