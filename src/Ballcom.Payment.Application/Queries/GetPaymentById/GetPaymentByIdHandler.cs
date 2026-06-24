using Ballcom.Payment.Application.DTOs;
using Ballcom.Payment.Application.Interfaces;

namespace Ballcom.Payment.Application.Queries.GetPaymentById;

public class GetPaymentByIdHandler(IPaymentReadRepository readRepository)
{
    public async Task<PaymentDto?> Handle(
        GetPaymentByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await readRepository.GetByIdAsync(query.PaymentId, cancellationToken);
    }
}