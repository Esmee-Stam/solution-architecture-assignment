using Ballcom.Order.Application.DTOs;
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
    ShoppingCartService shoppingCartService
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

    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult<ShoppingCartDto>> GetByCustomerId(Guid customerId)
    {
        var userId = GetCustomerId();

        if (userId != customerId)
        {
            return Forbid();
        }

        var cart = await shoppingCartService.GetByCustomerId(customerId);

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

            Guid orderId = await shoppingCartService.CheckoutAsync(customerId, model.PaymentMethod);

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
