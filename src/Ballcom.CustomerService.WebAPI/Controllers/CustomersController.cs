using Ballcom.CustomerService.Application.Queries.GetCustomerById;
using Ballcom.CustomerService.Application.Queries.GetCustomerByPhoneNumber;
using Ballcom.CustomerService.Application.Queries.GetCustomerOrders;
using Ballcom.CustomerService.Application.Queries.GetCustomerOverview;
using Ballcom.CustomerService.Application.Queries.GetCustomerShipments;
using Ballcom.CustomerService.Application.Queries.GetCustomers;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.CustomerService.WebAPI.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomersController(
    GetCustomersHandler getCustomersHandler,
    GetCustomerByIdHandler getCustomerByIdHandler,
    GetCustomerByPhoneNumberHandler getCustomerByPhoneNumberHandler,
    GetCustomerOrdersHandler getCustomerOrdersHandler,
    GetCustomerShipmentsHandler getCustomerShipmentsHandler,
    GetCustomerOverviewHandler getCustomerOverviewHandler) : ControllerBase
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

    [HttpGet("{customerId:guid}/orders")]
    public async Task<IActionResult> GetCustomerOrders(Guid customerId, CancellationToken cancellationToken = default)
    {
        var orders = await getCustomerOrdersHandler.Handle(new GetCustomerOrdersQuery(customerId), cancellationToken);
        return Ok(orders);
    }

    [HttpGet("{customerId:guid}/shipments")]
    public async Task<IActionResult> GetCustomerShipments(Guid customerId, CancellationToken cancellationToken = default)
    {
        var shipments = await getCustomerShipmentsHandler.Handle(new GetCustomerShipmentsQuery(customerId), cancellationToken);
        return Ok(shipments);
    }

    [HttpGet("{customerId:guid}/overview")]
    public async Task<IActionResult> GetCustomerOverview(Guid customerId, CancellationToken cancellationToken = default)
    {
        var overview = await getCustomerOverviewHandler.Handle(new GetCustomerOverviewQuery(customerId), cancellationToken);
        return overview is null ? NotFound() : Ok(overview);
    }
}
