using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Queries.GetOrderById;
using Ballcom.Order.Application.Queries.GetOrdersByCustomerId;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ballcom.Order.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(
    GetOrderByIdHandler getOrderByIdHandler,
    GetOrdersByCustomerIdHandler getOrdersByCustomerIdHandler) : ControllerBase
{
    private Guid GetCustomerId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var customerId))
        {
            throw new UnauthorizedAccessException("User is not authenticated or ID is invalid.");
        }

        return customerId;
    }

    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid orderId)
    {
        var customerId = GetCustomerId();

        var result = await getOrderByIdHandler.Handle(new GetOrderByIdQuery(orderId, customerId));

        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<OrderDto>>> GetByCustomerId()
    {
        var customerId = GetCustomerId();

        var result = await getOrdersByCustomerIdHandler.Handle(new GetOrdersByCustomerIdQuery(customerId));

        return Ok(result);
    }
}
