using Ballcom.Order.Application.Commands.PlaceOrder;
using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Orders.Commands;
using Ballcom.Order.Application.Services;
using Ballcom.Order.Domain.ValueObjects;
using Ballcom.Order.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ballcom.Order.WebApi.Controllers;

[ApiController]
[Route("api/shopping-carts")]
[Authorize(Roles = "Customer")]
public class ShoppingCartsController(
    ShoppingCartService shoppingCartService,
    PlaceOrderCommandHandler handler
    ) : ControllerBase
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

    [HttpGet]
    public async Task<ActionResult<ShoppingCartDto>> GetByCustomerId()
    {
        var userId = GetCustomerId();

        var cart = await shoppingCartService.GetByCustomerId(userId);

        if (cart is null) return NotFound();

        return Ok(cart);

    }

    [HttpDelete("items/{productId:guid}")]
    public async Task<ActionResult<ShoppingCartDto>> RemoveItem(Guid productId)
    {
        var customerId = GetCustomerId();

        var cart = await shoppingCartService.GetByCustomerId(customerId);

        if (cart is null) return NotFound();

        await shoppingCartService.RemoveProduct(cart.Id, productId);

        return NoContent();
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<OrderDto>> Checkout([FromBody] CheckoutCartModel model)
    {
        try
        {
            var customerId = GetCustomerId();

            var command = new PlaceOrderCommand(customerId, model.PaymentMethod);

            Guid orderId = await handler.Handle(command);

            return Ok(new
            {
                OrderId = orderId,
                Message = "Checkout completed successfully."
            });

        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
