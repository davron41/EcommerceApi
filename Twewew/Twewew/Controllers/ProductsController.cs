using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Twewew.DTOs;
using Twewew.Requests.Product;
using Twewew.Services.Interfaces;

namespace Twewew.Controllers;

[Route("api/products")]
[ApiController]


public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [HttpHead]
    public async Task<ActionResult<List<ProductDto>>> GetAsync()
    {
        var response = await _productService.GetAsync();

        return Ok(response);
    }
    [HttpGet("{id:int:min(1)}", Name = nameof(GetProductByIdAsync))]
    public async Task<ActionResult<ProductDto>> GetProductByIdAsync([FromRoute] ProductRequest request)
    {
        var response = await _productService.GetByIdAsync(request);

        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateAsync([FromBody] CreateProductRequest request)
    {
        var response = await _productService.CreateAsync(request);

        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> UpdateAsync(
        [FromRoute] int id,
        [FromBody] UpdateProductRequest request)
    {
        await _productService.UpdateAsync(request);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] ProductRequest request)
    {
        await _productService.DeleteAsync(request);

        return NoContent();
    }

    [HttpOptions]
    public IActionResult GetOptions()
    {
        string[] options = ["GET", "HEAD", "POST", "PUT", "DELETE"];

        HttpContext.Response.Headers.Append("X-Options", JsonConvert.SerializeObject(options));

        return Ok(options);


    }
}