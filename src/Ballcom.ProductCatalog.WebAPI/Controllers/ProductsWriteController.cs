using Ballcom.ProductCatalog.Application.Commands.CreateProduct;
using Ballcom.ProductCatalog.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.ProductCatalog.WebAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsWriteController(CreateProductHandler createProductHandler) : ControllerBase
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
    }
}