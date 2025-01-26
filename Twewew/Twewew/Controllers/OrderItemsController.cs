using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Twewew.DTOs;
using Twewew.Requests.OrderItem;
using Twewew.Services.Interfaces;

namespace Twewew.Controllers;

[Authorize]
[Route("api/order-items")]
[ApiController]
public class OrderItemsController : ControllerBase
{
    private readonly IOrderItemService _itemService;
    private readonly ICurrentUserService _userService;

    public OrderItemsController(IOrderItemService itemService, ICurrentUserService userService)
    {
        _itemService = itemService;
        _userService = userService;
    }
    [HttpGet]
    [HttpHead]
    public async Task<ActionResult<List<OrderItemDto>>> GetAsync()
    {
        var response = await _itemService.GetAsync();

        return Ok(response);
    }

    [HttpGet("{id:int:min(1)}", Name = nameof(GetByIdAsync))]
    public async Task<ActionResult<OrderItemDto>> GetByIdAsync([FromRoute] OrderItemRequest request)
    {
        var response = await _itemService.GetByIdAsync(request);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<OrderItemDto>> CreateAsync([FromBody] CreateOrderItemRequest request)
    {
        var response = await _itemService.CreateAsync(request);

        return Ok(response);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> UpdateAsync(
        [FromRoute] int id,
        [FromBody] UpdateOrderItemRequest request)
    {
        await _itemService.UpdateAsync(request);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] OrderItemRequest request)
    {
        await _itemService.DeleteAsync(request);

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
