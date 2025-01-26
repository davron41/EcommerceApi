using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Twewew.Entities;
using Twewew.Exceptions;
using Twewew.Persistence;
using Twewew.Requests.Product;
using Twewew.Services.Interfaces;

namespace Twewew.Services;

public sealed class BasketService : IBasketService
{
    private readonly IDistributedCache _cache;
    private readonly ApplicationDbContext _dbContext;
    private readonly TimeSpan _freezeDuration = TimeSpan.FromMinutes(10.0);

    public BasketService(IDistributedCache cache, ApplicationDbContext dbContext)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        string cacheKey = "products";

        // Check Redis for cached product data
        var cachedProducts = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedProducts))
        {
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(cachedProducts);
        }

        // Fetch products from the database if not in cache
        var products = await _dbContext.Products
            .Where(x => x.IsFrozen == false)
            .ToListAsync();

        // Cache the product list in Redis for future use
        await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(products), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        return products;
    }

    public async Task<Product> GetProductByIdAsync(ProductRequest request)
    {
        string cacheKey = $"product:{request.Id}";

        // Check Redis for cached product data
        var cachedProduct = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedProduct))
        {
            return JsonConvert.DeserializeObject<Product>(cachedProduct);
        }

        // Fetch the product from the database if not in cache
        var product = await _dbContext.Products.FindAsync(request.Id);
        if (product is null)
        {
            throw new ProductIsNotExistException("Product is not exist in current context");
        }


        // Cache the product in Redis for future use
        await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(product), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        return product;
    }

    // Freeze product logic (updates both Redis and DB)
    public async Task<bool> FreezeProductAsync(ProductRequest request, Guid userId)
    {
        string cacheKey = $"product:freeze:{request.Id}";

        // Check Redis for freeze status
        var frozenBy = await _cache.GetStringAsync(cacheKey);

        // If product is already frozen, check who froze it
        if (!string.IsNullOrEmpty(frozenBy))
        {
            if (frozenBy.Equals(userId.ToString()))
            {
                return true; // Product is already frozen by the current user
            }
            return false; // Product is frozen by another user
        }

        // Freeze product in Redis
        await _cache.SetStringAsync(cacheKey, userId.ToString(), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _freezeDuration
        });

        // Freeze product in the database
        var product = await _dbContext.Products.FindAsync(request.Id);
        if (product is not null)
        {
            product.IsFrozen = true;
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        return true;
    }

    // Unfreeze product logic (updates both Redis and DB)
    public async Task UnfreezeProductAsync(ProductRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        string cacheKey = $"product:freeze:{request.Id}";

        // Remove the freeze from Redis
        await _cache.RemoveAsync(cacheKey);

        // Unfreeze the product in the database
        var product = await _dbContext.Products.FindAsync(request.Id);
        if (product != null)
        {
            product.IsFrozen = false; // Update the product status in the database
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }
    }

}
