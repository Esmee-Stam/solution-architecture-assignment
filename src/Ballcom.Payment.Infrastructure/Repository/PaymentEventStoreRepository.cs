using System.Text.Json;
using Ballcom.Payment.Application.DTOs;
using Ballcom.Payment.Application.Interfaces;
using Ballcom.Payment.Infrastructure.Data.EventStore;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.Payment.Infrastructure.Repository;

public class PaymentEventStoreRepository(PaymentEventStoreDbContext dbContext)
    : IPaymentEventStoreRepository
{
    public async Task AppendAsync(
        Guid paymentId,
        string eventType,
        object eventData,
        CancellationToken cancellationToken = default)
    {
        var currentVersion = await dbContext.PaymentEvents
            .Where(e => e.PaymentId == paymentId)
            .CountAsync(cancellationToken);

        var paymentEvent = new PaymentEventEntity
        {
            Id = Guid.NewGuid(),
            PaymentId = paymentId,
            EventType = eventType,
            EventDataJson = JsonSerializer.Serialize(eventData),
            OccurredAt = DateTime.UtcNow,
            Version = currentVersion + 1
        };

        dbContext.PaymentEvents.Add(paymentEvent);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PaymentEventDto>> GetByPaymentIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.PaymentEvents
            .Where(e => e.PaymentId == paymentId)
            .OrderBy(e => e.Version)
            .Select(e => new PaymentEventDto
            {
                Id = e.Id,
                PaymentId = e.PaymentId,
                EventType = e.EventType,
                EventDataJson = e.EventDataJson,
                OccurredAt = e.OccurredAt,
                Version = e.Version
            })
            .ToListAsync(cancellationToken);
    }
}