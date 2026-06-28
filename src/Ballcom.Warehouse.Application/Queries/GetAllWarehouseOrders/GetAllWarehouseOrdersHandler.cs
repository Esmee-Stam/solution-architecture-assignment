using Ballcom.Warehouse.Application.DTOs;
using Ballcom.Warehouse.Application.Interfaces;

namespace Ballcom.Warehouse.Application.Queries.GetAllWarehouseOrders;

public class GetAllWarehouseOrdersHandler(IWarehouseOrderReadRepository readRepository)
{
    public async Task<IReadOnlyList<WarehouseOrderDto>> Handle(
        GetAllWarehouseOrdersQuery query,
        CancellationToken cancellationToken = default)
    {
        return await readRepository.GetAllAsync(cancellationToken);
    }
}
