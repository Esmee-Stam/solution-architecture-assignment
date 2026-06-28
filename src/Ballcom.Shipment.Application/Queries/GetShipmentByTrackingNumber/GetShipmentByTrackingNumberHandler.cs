using Ballcom.Shipment.Application.Interfaces;
using DTOs = Ballcom.Shipment.Application.DTOs;

namespace Ballcom.Shipment.Application.Queries.GetShipmentByTrackingNumber;

public class GetShipmentByTrackingNumberHandler(IShipmentReadRepository readRepository)
{
    public async Task<DTOs.ShipmentDto?> Handle(GetShipmentByTrackingNumberQuery query, CancellationToken cancellationToken = default)
    {
        return await readRepository.GetByTrackingNumberAsync(query.TrackingNumber, cancellationToken);
    }
}
