using Ballcom.Warehouse.Application.Queries.GetAllWarehouseOrders;
using Ballcom.Warehouse.Application.Queries.GetWarehouseOrderById;
using Ballcom.Warehouse.Application.Queries.GetWarehouseOrderByOrderId;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.Warehouse.WebAPI.Controllers;

[ApiController]
[Route("api/warehouse-orders")]
public class WarehouseOrdersQueryController(
    GetAllWarehouseOrdersHandler getAllWarehouseOrdersHandler,
    GetWarehouseOrderByIdHandler getWarehouseOrderByIdHandler,
    GetWarehouseOrderByOrderIdHandler getWarehouseOrderByOrderIdHandler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var warehouseOrders = await getAllWarehouseOrdersHandler.Handle(
            new GetAllWarehouseOrdersQuery(),
            cancellationToken);

        return Ok(warehouseOrders);
    }

    [HttpGet("{warehouseOrderId:guid}")]
    public async Task<IActionResult> GetById(
        Guid warehouseOrderId,
        CancellationToken cancellationToken)
    {
        var warehouseOrder = await getWarehouseOrderByIdHandler.Handle(
            new GetWarehouseOrderByIdQuery(warehouseOrderId),
            cancellationToken);

        if (warehouseOrder is null)
        {
            return NotFound();
        }

        return Ok(warehouseOrder);
    }

    [HttpGet("order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrderId(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var warehouseOrder = await getWarehouseOrderByOrderIdHandler.Handle(
            new GetWarehouseOrderByOrderIdQuery(orderId),
            cancellationToken);

        if (warehouseOrder is null)
        {
            return NotFound();
        }

        return Ok(warehouseOrder);
    }
}
