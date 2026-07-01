using Ballcom.Warehouse.Application.DTOs;
using Ballcom.Warehouse.Application.Interfaces;

namespace Ballcom.Warehouse.Application.Queries.GetWarehouseOrderByOrderId;

public class GetWarehouseOrderByOrderIdHandler(IWarehouseOrderReadRepository readRepository)
{
    public async Task<WarehouseOrderDto?> Handle(
        GetWarehouseOrderByOrderIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await readRepository.GetByOrderIdAsync(query.OrderId, cancellationToken);
    }
}
