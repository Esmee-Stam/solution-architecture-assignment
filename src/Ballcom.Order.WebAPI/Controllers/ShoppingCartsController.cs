using Ballcom.Order.Application.Commands.AddCartItem;
using Ballcom.Order.Application.Commands.CheckoutCart;
using Ballcom.Order.Application.Commands.RemoveCartItem;
using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Queries.GetShoppingCartByCustomerId;
using Ballcom.Order.Domain.ValueObjects;
using Ballcom.Order.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.Order.WebApi.Controllers;

[ApiController]
[Route("api/shopping-carts")]
public class ShoppingCartsController(
    AddCartItemHandler addCartItemHandler,
    RemoveCartItemHandler removeCartItemHandler,
    CheckoutCartHandler checkoutCartHandler,
    GetShoppingCartByCustomerIdHandler getShoppingCartByCustomerIdHandler) : ControllerBase
{
    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult<ShoppingCartDto>> GetByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        var result = await getShoppingCartByCustomerIdHandler.Handle(new GetShoppingCartByCustomerIdQuery(customerId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("items")]
    public async Task<ActionResult<ShoppingCartDto>> AddItem([FromBody] AddCartItemModel model, CancellationToken cancellationToken)
    {
        var result = await addCartItemHandler.Handle(new AddCartItemCommand(model.CustomerId, model.ProductId, model.Quantity), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("items")]
    public async Task<ActionResult<ShoppingCartDto>> RemoveItem([FromBody] RemoveCartItemModel model, CancellationToken cancellationToken)
    {
        var result = await removeCartItemHandler.Handle(new RemoveCartItemCommand(model.CustomerId, model.ProductId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<OrderDto>> Checkout([FromBody] CheckoutCartModel model, CancellationToken cancellationToken)
    {
        var result = await checkoutCartHandler.Handle(new CheckoutCartCommand(model.CustomerId, model.PaymentMethod), cancellationToken);
        return Ok(result);
    }
}
