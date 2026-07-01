using Ballcom.CustomerService.Application.Commands.CreateCustomer;
using Ballcom.CustomerService.Application.Commands.UpdateCustomer;
using Ballcom.CustomerService.Application.Commands.UpsertImportedCustomer;
using Ballcom.CustomerService.Application.Queries.GetCustomerById;
using Ballcom.CustomerService.Application.Queries.GetCustomerByPhoneNumber;
using Ballcom.CustomerService.Application.Queries.GetCustomers;
using Ballcom.CustomerService.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.CustomerService.WebAPI.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomersController(
    GetCustomersHandler getCustomersHandler,
    GetCustomerByIdHandler getCustomerByIdHandler,
    GetCustomerByPhoneNumberHandler getCustomerByPhoneNumberHandler,
    CreateCustomerHandler createCustomerHandler,
    UpdateCustomerHandler updateCustomerHandler,
    UpsertImportedCustomerHandler upsertImportedCustomerHandler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCustomers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var customers = await getCustomersHandler.Handle(new GetCustomersQuery(pageNumber, pageSize, search), cancellationToken);
        return Ok(customers);
    }

    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetCustomerById(Guid customerId, CancellationToken cancellationToken = default)
    {
        var customer = await getCustomerByIdHandler.Handle(new GetCustomerByIdQuery(customerId), cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpGet("phone/{phoneNumber}")]
    public async Task<IActionResult> GetCustomerByPhoneNumber(string phoneNumber, CancellationToken cancellationToken = default)
    {
        var customer = await getCustomerByPhoneNumberHandler.Handle(new GetCustomerByPhoneNumberQuery(phoneNumber), cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await createCustomerHandler.Handle(new CreateCustomerCommand(
                model.FirstName,
                model.LastName,
                model.CompanyName,
                model.PhoneNumber,
                model.Address,
                model.IdentityUserId), cancellationToken);

            return CreatedAtAction(nameof(GetCustomerById), new { customerId = result.CustomerId }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{customerId:guid}")]
    public async Task<IActionResult> UpdateCustomer(Guid customerId, [FromBody] UpdateCustomerModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            await updateCustomerHandler.Handle(new UpdateCustomerCommand(
                customerId,
                model.FirstName,
                model.LastName,
                model.CompanyName,
                model.PhoneNumber,
                model.Address,
                model.IdentityUserId), cancellationToken);

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPost("imported")]
    public async Task<IActionResult> UpsertImportedCustomer([FromBody] ImportCustomerModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await upsertImportedCustomerHandler.Handle(new UpsertImportedCustomerCommand(
                model.FirstName,
                model.LastName,
                model.CompanyName,
                model.PhoneNumber,
                model.Address,
                model.IdentityUserId), cancellationToken);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
