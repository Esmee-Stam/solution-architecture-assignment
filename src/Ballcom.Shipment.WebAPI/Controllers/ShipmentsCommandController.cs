using Ballcom.Shipment.Application.Commands.CreateShipmentFromPackedWarehouseOrder;
using Ballcom.Shipment.Application.Commands.DeliverShipment;
using Ballcom.Shipment.Application.Commands.DispatchShipment;
using Ballcom.Shipment.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.Shipment.WebAPI.Controllers;

[ApiController]
[Route("api/shipments")]
public class ShipmentsCommandController(
    CreateShipmentFromPackedWarehouseOrderHandler createShipmentFromPackedWarehouseOrderHandler,
    DispatchShipmentHandler dispatchShipmentHandler,
    DeliverShipmentHandler deliverShipmentHandler) : ControllerBase
{
    [HttpPost("from-packed-warehouse-order")]
    public async Task<IActionResult> CreateFromPackedWarehouseOrder(
        [FromBody] CreateShipmentModel model,
        CancellationToken cancellationToken)
    {
        var result = await createShipmentFromPackedWarehouseOrderHandler.Handle(
            new CreateShipmentFromPackedWarehouseOrderCommand(
                model.WarehouseOrderId,
                model.OrderId,
                model.CustomerId,
                model.Carrier),
            cancellationToken);

        return CreatedAtAction(
            actionName: nameof(ShipmentsQueryController.GetById),
            controllerName: "ShipmentsQuery",
            routeValues: new { shipmentId = result.ShipmentId },
            value: result);
    }

    [HttpPost("{shipmentId:guid}/dispatch")]
    public async Task<IActionResult> DispatchShipment(
        Guid shipmentId,
        CancellationToken cancellationToken)
    {
        await dispatchShipmentHandler.Handle(new DispatchShipmentCommand(shipmentId), cancellationToken);
        return NoContent();
    }

    [HttpPost("{shipmentId:guid}/deliver")]
    public async Task<IActionResult> DeliverShipment(
        Guid shipmentId,
        CancellationToken cancellationToken)
    {
        await deliverShipmentHandler.Handle(new DeliverShipmentCommand(shipmentId), cancellationToken);
        return NoContent();
    }
}
