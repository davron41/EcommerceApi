using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Twewew.DTOs;
using Twewew.Requests.Order;
using Twewew.Services.Interfaces;

namespace Twewew.Controllers;

[Authorize]
[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [HttpHead]
    public async Task<ActionResult<List<OrderDto>>> GetAsync()
    {
        var response = await _orderService.GetAsync();

        return Ok(response);
    }

    [HttpGet("{id:int:min(1)}", Name = nameof(GetOrderByIdAsync))]
    public async Task<ActionResult<OrderDto>> GetOrderByIdAsync([FromRoute] OrderRequest request)
    {
        var response = await _orderService.GetByIdAsync(request);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateAsync([FromBody] CreateOrderRequest request)
    {
        var response = await _orderService.CreateAsync(request);

        return Ok(response);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> UpdateAsync(
        [FromRoute] int id,
        [FromBody] UpdateOrderRequest request)
    {
        await _orderService.UpdateAsync(request);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] OrderRequest request)
    {
        await _orderService.DeleteAsync(request);

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

