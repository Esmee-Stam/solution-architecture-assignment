using Ballcom.Payment.Application.Commands.CompletePayment;
using Ballcom.Payment.Application.Commands.FailPayment;
using Ballcom.Payment.Application.Commands.RequestPayment;
using Ballcom.Payment.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ballcom.Payment.WebAPI.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize(Roles = "Customer")]
public class PaymentsCommandController(
    RequestPaymentHandler requestPaymentHandler,
    CompletePaymentHandler completePaymentHandler,
    FailPaymentHandler failPaymentHandler) : ControllerBase
{
    private Guid GetCustomerId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var customerId))
        {
            throw new UnauthorizedAccessException("User is not authenticated or ID is invalid.");
        }

        return customerId;
    }

    [HttpPost]
    public async Task<IActionResult> RequestPayment(
        [FromBody] RequestPaymentModel model,
        CancellationToken cancellationToken)
    {
        var customerId = GetCustomerId();
        var command = new RequestPaymentCommand(
            model.OrderId,
            customerId,
            model.Amount,
            model.Currency,
            model.PaymentMethod
        );

        var result = await requestPaymentHandler.Handle(command, cancellationToken);

        return CreatedAtAction(
            actionName: nameof(PaymentsQueryController.GetPaymentById),
            controllerName: "PaymentsQuery",
            routeValues: new { paymentId = result.PaymentId },
            value: result);
    }

    [HttpPost("{paymentId:guid}/complete")]
    public async Task<IActionResult> CompletePayment(
        Guid paymentId,
        CancellationToken cancellationToken)
    {
        var command = new CompletePaymentCommand(paymentId);

        await completePaymentHandler.Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPost("{paymentId:guid}/fail")]
    public async Task<IActionResult> FailPayment(
        Guid paymentId,
        [FromBody] FailPaymentModel model,
        CancellationToken cancellationToken)
    {
        var command = new FailPaymentCommand(paymentId, model.Reason);

        await failPaymentHandler.Handle(command, cancellationToken);

        return NoContent();
    }
}