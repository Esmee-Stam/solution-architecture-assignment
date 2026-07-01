using Ballcom.Warehouse.Application.Commands.CreateWarehouseOrderFromPaidOrder;
using Ballcom.Warehouse.Application.Commands.PackWarehouseOrder;
using Ballcom.Warehouse.Application.Commands.PickWarehouseOrder;
using Ballcom.Warehouse.WebAPI.Models;
using Events.OrderEvents.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.Warehouse.WebAPI.Controllers;

[ApiController]
[Route("api/warehouse-orders")]
public class WarehouseOrdersCommandController(
    CreateWarehouseOrderFromPaidOrderHandler createWarehouseOrderFromPaidOrderHandler,
    PickWarehouseOrderHandler pickWarehouseOrderHandler,
    PackWarehouseOrderHandler packWarehouseOrderHandler) : ControllerBase
{
    [HttpPost("from-paid-order")]
    public async Task<IActionResult> CreateFromPaidOrder(
        [FromBody] CreateWarehouseOrderModel model,
        CancellationToken cancellationToken)
    {
        var command = new CreateWarehouseOrderFromPaidOrderCommand(
            model.OrderId,
            model.CustomerId,
            model.PaymentMethod,
            model.TotalAmount,
            model.Currency,
            model.Items.Select(item => new OrderItemEventDto(
                Guid.NewGuid(),
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice,
                item.Currency
            )).ToList()
        );

        var result = await createWarehouseOrderFromPaidOrderHandler.Handle(command, cancellationToken);

        return CreatedAtAction(
            actionName: nameof(WarehouseOrdersQueryController.GetById),
            controllerName: "WarehouseOrdersQuery",
            routeValues: new { warehouseOrderId = result.WarehouseOrderId },
            value: result);
    }

    [HttpPost("{warehouseOrderId:guid}/pick")]
    public async Task<IActionResult> PickWarehouseOrder(
        Guid warehouseOrderId,
        CancellationToken cancellationToken)
    {
        await pickWarehouseOrderHandler.Handle(
            new PickWarehouseOrderCommand(warehouseOrderId),
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{warehouseOrderId:guid}/pack")]
    public async Task<IActionResult> PackWarehouseOrder(
        Guid warehouseOrderId,
        CancellationToken cancellationToken)
    {
        await packWarehouseOrderHandler.Handle(
            new PackWarehouseOrderCommand(warehouseOrderId),
            cancellationToken);

        return NoContent();
    }
}
