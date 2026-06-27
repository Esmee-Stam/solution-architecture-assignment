using Ballcom.Payment.Application.Queries.GetPaymentById;
using Ballcom.Payment.Application.Queries.GetPaymentByOrderId;
using Ballcom.Payment.Application.Queries.GetPaymentEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.Payment.WebAPI.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize(Roles = "Customer")]

public class PaymentsQueryController(
    GetPaymentByIdHandler getPaymentByIdHandler,
    GetPaymentByOrderIdHandler getPaymentByOrderIdHandler,
    GetPaymentEventsHandler getPaymentEventsHandler) : ControllerBase
{
    [HttpGet("{paymentId:guid}")]
    public async Task<IActionResult> GetPaymentById(
        Guid paymentId,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentByIdQuery(paymentId);

        var payment = await getPaymentByIdHandler.Handle(query, cancellationToken);

        if (payment is null)
        {
            return NotFound();
        }

        return Ok(payment);
    }

    [HttpGet("order/{orderId:guid}")]
    public async Task<IActionResult> GetPaymentByOrderId(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentByOrderIdQuery(orderId);

        var payment = await getPaymentByOrderIdHandler.Handle(query, cancellationToken);

        if (payment is null)
        {
            return NotFound();
        }

        return Ok(payment);
    }

    [HttpGet("{paymentId:guid}/events")]
    public async Task<IActionResult> GetPaymentEvents(
        Guid paymentId,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentEventsQuery(paymentId);

        var events = await getPaymentEventsHandler.Handle(query, cancellationToken);

        return Ok(events);
    }
}