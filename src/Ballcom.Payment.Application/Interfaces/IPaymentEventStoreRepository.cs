using Ballcom.Payment.Application.DTOs;

namespace Ballcom.Payment.Application.Interfaces;

public interface IPaymentEventStoreRepository
{
    Task AppendAsync(
        Guid paymentId,
        string eventType,
        object eventData,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PaymentEventDto>> GetByPaymentIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default);
}