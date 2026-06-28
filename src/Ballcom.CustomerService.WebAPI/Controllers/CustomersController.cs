using Ballcom.CustomerService.Application;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.CustomerService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(ICustomerRepository repository) : ControllerBase
    {
        public async Task<IActionResult> GetCustomers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var customers = await repository.GetCustomersAsync(pageNumber, pageSize);
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await repository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
    }
}
