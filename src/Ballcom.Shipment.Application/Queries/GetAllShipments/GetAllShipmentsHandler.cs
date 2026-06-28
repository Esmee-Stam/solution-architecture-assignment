using Ballcom.Shipment.Application.Interfaces;
using DTOs = Ballcom.Shipment.Application.DTOs;

namespace Ballcom.Shipment.Application.Queries.GetAllShipments;

public class GetAllShipmentsHandler(IShipmentReadRepository readRepository)
{
    public async Task<IReadOnlyList<DTOs.ShipmentDto>> Handle(GetAllShipmentsQuery query, CancellationToken cancellationToken = default)
    {
        return await readRepository.GetAllAsync(cancellationToken);
    }
}
