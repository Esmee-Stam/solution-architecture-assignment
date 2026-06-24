using Ballcom.Payment.Application.DTOs;
using Ballcom.Payment.Application.Interfaces;

namespace Ballcom.Payment.Application.Queries.GetPaymentEvents;

public class GetPaymentEventsHandler(IPaymentEventStoreRepository eventStoreRepository)
{
    public async Task<IReadOnlyList<PaymentEventDto>> Handle(
        GetPaymentEventsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await eventStoreRepository.GetByPaymentIdAsync(
            query.PaymentId,
            cancellationToken);
    }
}