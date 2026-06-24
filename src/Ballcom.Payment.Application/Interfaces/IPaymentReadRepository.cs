using Ballcom.Payment.Application.DTOs;

namespace Ballcom.Payment.Application.Interfaces;

public interface IPaymentReadRepository
{
    Task<PaymentDto?> GetByIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default);

    Task<PaymentDto?> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task UpsertAsync(
        PaymentDto payment,
        CancellationToken cancellationToken = default);
}