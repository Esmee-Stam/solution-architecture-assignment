using Ballcom.Warehouse.Application.DTOs;
using Ballcom.Warehouse.Application.Interfaces;

namespace Ballcom.Warehouse.Application.Queries.GetWarehouseOrderById;

public class GetWarehouseOrderByIdHandler(IWarehouseOrderReadRepository readRepository)
{
    public async Task<WarehouseOrderDto?> Handle(
        GetWarehouseOrderByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await readRepository.GetByIdAsync(query.WarehouseOrderId, cancellationToken);
    }
}
