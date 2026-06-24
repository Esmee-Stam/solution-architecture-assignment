using Ballcom.Payment.Application.Commands.CompletePayment;
using Ballcom.Payment.Application.Commands.FailPayment;
using Ballcom.Payment.Application.Commands.RequestPayment;
using Ballcom.Payment.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ballcom.Payment.WebAPI.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsCommandController(
    RequestPaymentHandler requestPaymentHandler,
    CompletePaymentHandler completePaymentHandler,
    FailPaymentHandler failPaymentHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RequestPayment(
        [FromBody] RequestPaymentModel model,
        CancellationToken cancellationToken)
    {
        var command = new RequestPaymentCommand(
            model.OrderId,
            model.CustomerId,
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