using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Queries.GetOrderById;
using Ballcom.Order.Application.Queries.GetOrdersByCustomerId;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.Order.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(
    GetOrderByIdHandler getOrderByIdHandler,
    GetOrdersByCustomerIdHandler getOrdersByCustomerIdHandler) : ControllerBase
{
    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid orderId, CancellationToken cancellationToken)
    {
        var result = await getOrderByIdHandler.Handle(new GetOrderByIdQuery(orderId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<ActionResult<IReadOnlyCollection<OrderDto>>> GetByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        var result = await getOrdersByCustomerIdHandler.Handle(new GetOrdersByCustomerIdQuery(customerId), cancellationToken);
        return Ok(result);
    }
}
