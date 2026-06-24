using Ballcom.ProductCatalog.Application.Commands.CreateProduct;
using Ballcom.ProductCatalog.Application.Queries.GetProductById;
using Ballcom.ProductCatalog.WebAPI.Models;
using Events.ProductCatalogEvents;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ballcom.ProductCatalog.WebAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsWriteController(
        CreateProductHandler createProductHandler,
        GetProductByIdHandler productByIdHandler,
        IPublishEndpoint publishEndpoint
        ) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductModel model)
        {
            try
            {
                var command = new CreateProductCommand(
                    model.Name,
                    model.Description,
                    model.PriceAmount,
                    model.Currency,
                    model.Stock
                );

                var result = await createProductHandler.Handle(command);

                return CreatedAtAction(nameof(CreateProduct), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-to-cart")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddToCartModel model)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(customerId)) return Unauthorized("Customer ID not found in claims.");

            var product = await productByIdHandler.Handle(new GetProductById(model.ProductId));

            if (product == null) return NotFound("Product not found.");

            var cartEvent = new ProductAddedToCartEvent(
                CustomerId: customerId,
                ProductId: product.Id,
                ProductName: product.Name,
                Price: product.PriceAmount,
                Currency: product.Currency,
                Quantity: model.Quantity
            );

            await publishEndpoint.Publish(cartEvent);

            return Ok(new { Message = "Product added to cart successfully." });
        }
    }
}