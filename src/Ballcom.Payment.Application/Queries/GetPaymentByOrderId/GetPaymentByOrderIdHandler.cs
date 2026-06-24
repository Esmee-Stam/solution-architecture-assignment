using Ballcom.Payment.Application.DTOs;
using Ballcom.Payment.Application.Interfaces;

namespace Ballcom.Payment.Application.Queries.GetPaymentByOrderId;

public class GetPaymentByOrderIdHandler(IPaymentReadRepository readRepository)
{
    public async Task<PaymentDto?> Handle(
        GetPaymentByOrderIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await readRepository.GetByOrderIdAsync(query.OrderId, cancellationToken);
    }
}