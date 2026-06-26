using Ballcom.Payment.Application.Commands.RequestPayment;
using Events.OrderEvents;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ballcom.Payment.Infrastructure.Messaging
{
    public class OrderPlacedConsumer(RequestPaymentHandler requestPaymentHandler) : IConsumer<OrderPlacedEvent>
    {
        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;

            var command = new RequestPaymentCommand(
                message.OrderId,
                message.CustomerId,
                message.TotalAmount,
                message.Currency,
                message.PaymentMethod
            );

            var result = await requestPaymentHandler.Handle(command, CancellationToken.None);

            Console.WriteLine($"Payment requested for OrderId: {message.OrderId}, PaymentId: {result.PaymentId}");
        }
    }
}
