using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Twewew.DTOs;
using Twewew.Requests.Category;
using Twewew.Services.Interfaces;

namespace Twewew.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public CategoriesController(ICategoryService categoryService, IProductService productService)
    {
        _categoryService = categoryService;
        _productService = productService;
    }
    [HttpGet]
    [HttpHead]
    public async Task<ActionResult<List<CategoryDto>>> GetAsync()
    {
        var categories = await _categoryService.GetAsync();

        return Ok(categories);
    }

    [HttpGet("{id:int:min(1)}", Name = nameof(GetCategoryByIdAsync))]
    public async Task<ActionResult<CategoryDto>> GetCategoryByIdAsync([FromRoute] CategoryRequest request)
    {
        var category = await _categoryService.GetByIdAsync(request);

        return Ok(category);
    }

    [HttpGet("{id:int:min(1)}/products")]
    public async Task<ActionResult<List<ProductDto>>> GetProductsAsync([FromRoute] CategoryRequest request)
    {
        var response = await _categoryService.GetByCategoryAsync(request);

        return Ok(response);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateAsync([FromBody] CreateCategoryRequest request)
    {
        var response = await _categoryService.CreateAsync(request);

        return CreatedAtAction(nameof(GetCategoryByIdAsync), new { id = response.Id }, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> UpdateAsync(
        [FromRoute] int id,
        [FromBody] UpdateCategoryRequest request)
    {
        await _categoryService.UpdateAsync(request);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] CategoryRequest request)
    {
        await _categoryService.DeleteAsync(request);

        return NoContent();
    }

    [HttpOptions]
    public IActionResult GetOptions()
    {
        string[] options = ["GET", "POST", "PUT", "DELETE", "HEAD"];

        HttpContext.Response.Headers.Append("X-Options", JsonConvert.SerializeObject(options));

        return Ok(options);
    }


}
