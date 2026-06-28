using Ballcom.Shipment.Application.Interfaces;
using DTOs = Ballcom.Shipment.Application.DTOs;

namespace Ballcom.Shipment.Application.Queries.GetShipmentById;

public class GetShipmentByIdHandler(IShipmentReadRepository readRepository)
{
    public async Task<DTOs.ShipmentDto?> Handle(GetShipmentByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await readRepository.GetByIdAsync(query.ShipmentId, cancellationToken);
    }
}
