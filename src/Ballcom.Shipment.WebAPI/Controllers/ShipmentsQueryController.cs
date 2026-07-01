using Ballcom.Shipment.Application.Queries.GetAllShipments;
using Ballcom.Shipment.Application.Queries.GetShipmentById;
using Ballcom.Shipment.Application.Queries.GetShipmentByOrderId;
using Ballcom.Shipment.Application.Queries.GetShipmentByTrackingNumber;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.Shipment.WebAPI.Controllers;

[ApiController]
[Route("api/shipments")]
public class ShipmentsQueryController(
    GetAllShipmentsHandler getAllShipmentsHandler,
    GetShipmentByIdHandler getShipmentByIdHandler,
    GetShipmentByOrderIdHandler getShipmentByOrderIdHandler,
    GetShipmentByTrackingNumberHandler getShipmentByTrackingNumberHandler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var shipments = await getAllShipmentsHandler.Handle(new GetAllShipmentsQuery(), cancellationToken);
        return Ok(shipments);
    }

    [HttpGet("{shipmentId:guid}")]
    public async Task<IActionResult> GetById(Guid shipmentId, CancellationToken cancellationToken)
    {
        var shipment = await getShipmentByIdHandler.Handle(new GetShipmentByIdQuery(shipmentId), cancellationToken);
        return shipment is null ? NotFound() : Ok(shipment);
    }

    [HttpGet("order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrderId(Guid orderId, CancellationToken cancellationToken)
    {
        var shipment = await getShipmentByOrderIdHandler.Handle(new GetShipmentByOrderIdQuery(orderId), cancellationToken);
        return shipment is null ? NotFound() : Ok(shipment);
    }

    [HttpGet("tracking/{trackingNumber}")]
    public async Task<IActionResult> GetByTrackingNumber(string trackingNumber, CancellationToken cancellationToken)
    {
        var shipment = await getShipmentByTrackingNumberHandler.Handle(new GetShipmentByTrackingNumberQuery(trackingNumber), cancellationToken);
        return shipment is null ? NotFound() : Ok(shipment);
    }
}
