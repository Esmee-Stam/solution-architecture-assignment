using Ballcom.ProductCatalog.Application.Queries.GetAllProducts;
using Ballcom.ProductCatalog.Application.Queries.GetProductById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.ProductCatalog.WebAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsReadController(
        GetAllProductsHandler getAllHandler,
        GetProductByIdHandler getByIdHandler
        ) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await getAllHandler.Handle(new GetAllProductsQuery());

            return Ok(products);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await getByIdHandler.Handle(new GetProductById(id));

            if (product == null) return NotFound();

            return Ok(product);
        }
    }
}
