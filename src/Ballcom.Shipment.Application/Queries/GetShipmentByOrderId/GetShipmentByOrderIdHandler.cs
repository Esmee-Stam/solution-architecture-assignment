using Ballcom.Shipment.Application.Interfaces;
using DTOs = Ballcom.Shipment.Application.DTOs;

namespace Ballcom.Shipment.Application.Queries.GetShipmentByOrderId;

public class GetShipmentByOrderIdHandler(IShipmentReadRepository readRepository)
{
    public async Task<DTOs.ShipmentDto?> Handle(GetShipmentByOrderIdQuery query, CancellationToken cancellationToken = default)
    {
        return await readRepository.GetByOrderIdAsync(query.OrderId, cancellationToken);
    }
}
