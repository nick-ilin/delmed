using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Features.Orders;
using Order.Infrastructure.Entities.Enums;

namespace Order.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateOrderResponse>> CreateOrder(CreateOrderCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetOrderResponse>> GetOrder(int id)
    {
        var result = await mediator.Send(new GetOrderQuery(id));
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPut("{id}/status/{status}")]
    public async Task<ActionResult<GetOrderResponse>> ChangeOrderStatus(int id, OrderStatus status)
    {
        var result = await mediator.Send(new ChangeOrderStatusCommand(id, status));
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}