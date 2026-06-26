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
    CompletePaymentHandler completePaymentHandler,
    FailPaymentHandler failPaymentHandler) : ControllerBase
{
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