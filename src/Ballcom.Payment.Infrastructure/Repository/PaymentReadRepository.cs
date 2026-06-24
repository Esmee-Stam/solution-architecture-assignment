using Ballcom.Payment.Application.DTOs;
using Ballcom.Payment.Application.Interfaces;
using Ballcom.Payment.Infrastructure.Data.Read;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Payment.Infrastructure.Repository;

public class PaymentReadRepository(PaymentReadDbContext dbContext)
    : IPaymentReadRepository
{
    public async Task<PaymentDto?> GetByIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Payments
            .Where(p => p.PaymentId == paymentId)
            .Select(p => ToDto(p))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PaymentDto?> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Payments
            .Where(p => p.OrderId == orderId)
            .Select(p => ToDto(p))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpsertAsync(
        PaymentDto payment,
        CancellationToken cancellationToken = default)
    {
        var existingPayment = await dbContext.Payments
            .FirstOrDefaultAsync(p => p.PaymentId == payment.PaymentId, cancellationToken);

        if (existingPayment is null)
        {
            var readModel = new PaymentReadModel
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                CustomerId = payment.CustomerId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status,
                FailureReason = payment.FailureReason,
                CreatedAt = payment.CreatedAt,
                LastUpdatedAt = payment.LastUpdatedAt
            };

            dbContext.Payments.Add(readModel);
        }
        else
        {
            existingPayment.OrderId = payment.OrderId;
            existingPayment.CustomerId = payment.CustomerId;
            existingPayment.Amount = payment.Amount;
            existingPayment.Currency = payment.Currency;
            existingPayment.PaymentMethod = payment.PaymentMethod;
            existingPayment.Status = payment.Status;
            existingPayment.FailureReason = payment.FailureReason;
            existingPayment.LastUpdatedAt = payment.LastUpdatedAt;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static PaymentDto ToDto(PaymentReadModel payment)
    {
        return new PaymentDto
        {
            PaymentId = payment.PaymentId,
            OrderId = payment.OrderId,
            CustomerId = payment.CustomerId,
            Amount = payment.Amount,
            Currency = payment.Currency,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status,
            FailureReason = payment.FailureReason,
            CreatedAt = payment.CreatedAt,
            LastUpdatedAt = payment.LastUpdatedAt
        };
    }
}