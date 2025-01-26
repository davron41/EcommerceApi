using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twewew.DTOs;
using Twewew.Requests.Product;
using Twewew.Services.Interfaces;

namespace Twewew.Controllers;

[Authorize]
[Route("api/basket")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly ICurrentUserService _userService;
    private readonly IBasketService _basketService;
    private readonly IMapper _mapper;

    public BasketController(ICurrentUserService userService, IBasketService basketService, IMapper mapper)
    {
        _userService = userService;
        _basketService = basketService;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAsync()
    {
        var response = await _basketService.GetProductsAsync();

        return Ok(_mapper.Map<ProductDto>(response));
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<ProductDto>> GetByIdAsync(ProductRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await _basketService.GetProductByIdAsync(request);

        return Ok(_mapper.Map<ProductDto>(response));
    }

    [Authorize]
    [HttpPost("freeze")]
    public async Task<ActionResult> FreezeProduct(ProductRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var userId = _userService.GetUserId();

        var success = await _basketService.FreezeProductAsync(request, userId);

        if (!success)
        {
            return BadRequest("This product is currently frozen by another user");
        }

        return Ok(success);
    }

    [Authorize]
    [HttpDelete("unfreeze/{id:int:min(1)}")]
    public async Task<ActionResult> UnfreezeProduct(ProductRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        await _basketService.UnfreezeProductAsync(request);

        return Ok("Product unfrozen successfully");
    }
}
