using Ballcom.CustomerService.Application;
using Ballcom.CustomerService.Application.DTOs;
using Ballcom.CustomerService.Domain.Domain;
using Ballcom.CustomerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ballcom.CustomerService.Infrastructure.Repository;

public class CustomerRepository(CustomerServiceDbContext context) : ICustomerRepository
{
    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await context.Customers.AddAsync(customer, cancellationToken);
    }

    public async Task<Customer?> GetByIdEntityAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Customers.FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);
    }

    public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await context.Customers.FirstOrDefaultAsync(customer => customer.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<Customer?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken = default)
    {
        return await context.Customers.FirstOrDefaultAsync(customer => customer.IdentityUserId == identityUserId, cancellationToken);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Customers
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CustomerDto(
                c.Id,
                c.FirstName,
                c.LastName,
                c.FirstName + " " + c.LastName,
                c.PhoneNumber,
                c.CompanyName,
                c.Address,
                c.IdentityUserId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CustomerDto?> GetCustomerByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await context.Customers
            .AsNoTracking()
            .Where(c => c.PhoneNumber == phoneNumber)
            .Select(c => new CustomerDto(
                c.Id,
                c.FirstName,
                c.LastName,
                c.FirstName + " " + c.LastName,
                c.PhoneNumber,
                c.CompanyName,
                c.Address,
                c.IdentityUserId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<CustomerDto>> GetCustomersAsync(
        int pageNumber,
        int pageSize,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Customers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();
            query = query.Where(c =>
                c.FirstName.Contains(normalizedSearch)
                || c.LastName.Contains(normalizedSearch)
                || (c.CompanyName != null && c.CompanyName.Contains(normalizedSearch))
                || (c.PhoneNumber != null && c.PhoneNumber.Contains(normalizedSearch))
                || (c.Address != null && c.Address.Contains(normalizedSearch)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CustomerDto(
                c.Id,
                c.FirstName,
                c.LastName,
                c.FirstName + " " + c.LastName,
                c.PhoneNumber,
                c.CompanyName,
                c.Address,
                c.IdentityUserId))
            .ToListAsync(cancellationToken);

        return new PagedResult<CustomerDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<List<CustomerOrderDto>> GetCustomerOrdersAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var orders = await context.CustomerOrders
            .AsNoTracking()
            .Include(order => order.Items)
            .Where(order => order.CustomerId == customerId)
            .OrderByDescending(order => order.LastStatusChangedAt ?? order.PlacedAt)
            .ToListAsync(cancellationToken);

        return orders.Select(ToDto).ToList();
    }

    public async Task<List<CustomerShipmentDto>> GetCustomerShipmentsAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var shipments = await context.CustomerShipments
            .AsNoTracking()
            .Where(shipment => shipment.CustomerId == customerId)
            .OrderByDescending(shipment => shipment.LastUpdatedAt)
            .ToListAsync(cancellationToken);

        return shipments.Select(ToDto).ToList();
    }

    public async Task<CustomerOverviewDto?> GetCustomerOverviewAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        // The CustomerService record has its own Id, but orders and shipments use the Identity user id
        // as CustomerId because they originate from authenticated customer actions.
        // Therefore the overview accepts both ids:
        // - CustomerService record id, for selecting the customer record
        // - Identity user id, for account based lookups
        var customerEntity = await context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                customer => customer.Id == customerId || customer.IdentityUserId == customerId.ToString(),
                cancellationToken);

        if (customerEntity is null)
        {
            return null;
        }

        var readModelCustomerId = Guid.TryParse(customerEntity.IdentityUserId, out var identityUserId)
            ? identityUserId
            : customerEntity.Id;

        var customer = ToDto(customerEntity);
        var orders = await GetCustomerOrdersAsync(readModelCustomerId, cancellationToken);
        var shipments = await GetCustomerShipmentsAsync(readModelCustomerId, cancellationToken);

        return new CustomerOverviewDto(customer, orders, shipments);
    }

    public async Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid? excludedCustomerId = null, CancellationToken cancellationToken = default)
    {
        return await context.Customers.AnyAsync(customer =>
            customer.PhoneNumber == phoneNumber
            && (!excludedCustomerId.HasValue || customer.Id != excludedCustomerId.Value), cancellationToken);
    }

    public async Task UpsertOrderAsync(CustomerOrder order, CancellationToken cancellationToken = default)
    {
        var existingOrder = await context.CustomerOrders
            .FirstOrDefaultAsync(existing => existing.OrderId == order.OrderId, cancellationToken);

        if (existingOrder is null)
        {
            await context.CustomerOrders.AddAsync(order, cancellationToken);
            return;
        }

        existingOrder.CustomerId = order.CustomerId;
        existingOrder.Status = order.Status;
        existingOrder.TotalAmount = order.TotalAmount;
        existingOrder.Currency = order.Currency;
        existingOrder.PaymentMethod = order.PaymentMethod;
        existingOrder.PlacedAt = order.PlacedAt ?? existingOrder.PlacedAt;
        existingOrder.LastStatusChangedAt = order.LastStatusChangedAt ?? existingOrder.LastStatusChangedAt;

        // OrderStatusChanged events only need to update the order status/read model.
        // They should not replace order lines, because that can conflict with the OrderPlaced consumer
        // when events are processed close to each other.
        if (order.Items.Count == 0)
        {
            return;
        }

        await context.Entry(existingOrder)
            .Collection(existing => existing.Items)
            .LoadAsync(cancellationToken);

        var incomingItemsById = order.Items.ToDictionary(item => item.Id);

        foreach (var existingItem in existingOrder.Items.ToList())
        {
            if (!incomingItemsById.TryGetValue(existingItem.Id, out var incomingItem))
            {
                context.CustomerOrderItems.Remove(existingItem);
                continue;
            }

            existingItem.ProductId = incomingItem.ProductId;
            existingItem.ProductName = incomingItem.ProductName;
            existingItem.Quantity = incomingItem.Quantity;
            existingItem.UnitPrice = incomingItem.UnitPrice;
            existingItem.Currency = incomingItem.Currency;
        }

        var existingItemIds = existingOrder.Items.Select(item => item.Id).ToHashSet();
        foreach (var incomingItem in order.Items.Where(item => !existingItemIds.Contains(item.Id)))
        {
            existingOrder.Items.Add(incomingItem);
        }
    }

    public async Task UpsertShipmentAsync(CustomerShipment shipment, CancellationToken cancellationToken = default)
    {
        var existingShipment = await context.CustomerShipments
            .FirstOrDefaultAsync(existing => existing.ShipmentId == shipment.ShipmentId, cancellationToken);

        if (existingShipment is null)
        {
            await context.CustomerShipments.AddAsync(shipment, cancellationToken);
            return;
        }

        existingShipment.WarehouseOrderId = shipment.WarehouseOrderId == Guid.Empty ? existingShipment.WarehouseOrderId : shipment.WarehouseOrderId;
        existingShipment.OrderId = shipment.OrderId;
        existingShipment.CustomerId = shipment.CustomerId;
        existingShipment.Status = shipment.Status;
        existingShipment.Carrier = shipment.Carrier;
        existingShipment.TrackingNumber = shipment.TrackingNumber;
        existingShipment.CreatedAt = shipment.CreatedAt ?? existingShipment.CreatedAt;
        existingShipment.DispatchedAt = shipment.DispatchedAt ?? existingShipment.DispatchedAt;
        existingShipment.DeliveredAt = shipment.DeliveredAt ?? existingShipment.DeliveredAt;
        existingShipment.LastUpdatedAt = shipment.LastUpdatedAt;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    private static CustomerDto ToDto(Customer customer)
    {
        return new CustomerDto(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            $"{customer.FirstName} {customer.LastName}".Trim(),
            customer.PhoneNumber,
            customer.CompanyName,
            customer.Address,
            customer.IdentityUserId);
    }

    private static CustomerOrderDto ToDto(CustomerOrder order)
    {
        return new CustomerOrderDto(
            order.OrderId,
            order.CustomerId,
            order.Status,
            order.TotalAmount,
            order.Currency,
            order.PaymentMethod,
            order.PlacedAt,
            order.LastStatusChangedAt,
            order.Items
                .OrderBy(item => item.ProductName)
                .Select(item => new CustomerOrderItemDto(
                    item.Id,
                    item.ProductId,
                    item.ProductName,
                    item.Quantity,
                    item.UnitPrice,
                    item.Currency))
                .ToList());
    }

    private static CustomerShipmentDto ToDto(CustomerShipment shipment)
    {
        return new CustomerShipmentDto(
            shipment.ShipmentId,
            shipment.WarehouseOrderId,
            shipment.OrderId,
            shipment.CustomerId,
            shipment.Status,
            shipment.Carrier,
            shipment.TrackingNumber,
            shipment.CreatedAt,
            shipment.DispatchedAt,
            shipment.DeliveredAt,
            shipment.LastUpdatedAt);
    }
}
