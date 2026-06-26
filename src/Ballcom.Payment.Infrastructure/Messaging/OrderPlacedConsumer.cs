using Ballcom.Payment.Application.Commands.RequestPayment;
using Events.OrderEvents;
using MassTransit;

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
        }
    }
}
