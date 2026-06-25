using Ballcom.Order.Application.Commands.CheckoutCart;
using Ballcom.Order.Application.Commands.RemoveCartItem;
using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Queries.GetShoppingCartByCustomerId;
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
    RemoveCartItemHandler removeCartItemHandler,
    CheckoutCartHandler checkoutCartHandler,
    GetShoppingCartByCustomerIdHandler getShoppingCartByCustomerIdHandler) : ControllerBase
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

    // Deze aanpassen
    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult<ShoppingCartDto>> GetByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        var userID = GetCustomerId();

        if (userID != customerId)
        {
            return Forbid("You are not authorized to access this shopping cart.");
        }

        var result = await getShoppingCartByCustomerIdHandler.Handle(new GetShoppingCartByCustomerIdQuery(customerId));

        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("items/{productId:guid}")]
    public async Task<ActionResult<ShoppingCartDto>> RemoveItem(Guid productId)
    {
        var customerId = GetCustomerId();

        await removeCartItemHandler.Handle(new RemoveCartItemCommand(customerId, productId));

        return NoContent();
    }

    // Deze aanpassen
    [HttpPost("checkout")]
    public async Task<ActionResult<OrderDto>> Checkout([FromBody] CheckoutCartModel model, CancellationToken cancellationToken)
    {
        var result = await checkoutCartHandler.Handle(new CheckoutCartCommand(model.CustomerId, model.PaymentMethod), cancellationToken);
        return Ok(result);
    }
}
