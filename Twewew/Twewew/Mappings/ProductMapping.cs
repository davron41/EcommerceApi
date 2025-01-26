using AutoMapper;
using Twewew.DTOs;
using Twewew.Entities;
using Twewew.Requests.Product;

namespace Twewew.Mappings;

public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductRequest, Product>();
        CreateMap<UpdateProductRequest, Product>();
    }
}
