using AutoMapper;
using Twewew.DTOs;
using Twewew.Entities;
using Twewew.Requests.Category;

namespace Twewew.Mappings;

public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<UpdateCategoryRequest, Category>();
    }
}
