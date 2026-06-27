using Ballcom.Order.Application.DTOs;
using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Infrastructure.Data.Read;
using Microsoft.EntityFrameworkCore;
using OrderAggregate = Ballcom.Order.Domain.Domain.Order;

namespace Ballcom.Order.Infrastructure.Repository;

public class OrderReadRepository(OrderReadDbContext context) : IOrderReadRepository
{
    public async Task<OrderDto?> GetByIdAsync(Guid orderId)
    {
        var order = await context.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        return order is null ? null : ToDto(order);
    }

    public async Task<IEnumerable<OrderDto>> GetOrderByCustomerIdAsync(Guid customerId)
    {
        var orders = await context.Orders.Include(x => x.OrderItems)
            .Where(x => x.CustomerId == customerId)
            .AsNoTracking()
              .Select(o => new OrderDto
              {
                  Id = o.Id,
                  CustomerId = o.CustomerId,
                  PaymentMethod = o.PaymentMethod,
                  Status = o.Status,
                  TotalAmount = o.TotalAmount,
                  CreatedAt = o.CreatedAt,

                  OrderItems = o.OrderItems.Select(i => new OrderItemDto
                  {
                      ProductId = i.ProductId,
                      ProductName = i.ProductName,
                      Quantity = i.Quantity,
                      UnitPrice = i.UnitPrice,
                      TotalPrice = i.TotalPrice
                  }).ToList()
              })
            .ToListAsync();

        return orders;
    }

    public async Task UpsertAsync(OrderDto order)
    {
        var existingOrder = await context.Orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.Id == order.Id);

        if (existingOrder is null)
        {
            var newOrder = new OrderReadModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Currency = order.Currency,
                CreatedAt = order.CreatedAt,

                OrderItems = order.OrderItems.Select(i => new OrderItemReadModel
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,

                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                    Currency = i.Currency
                }).ToList()
            };

            context.Orders.Add(newOrder);
        }
        else
        {
            existingOrder.PaymentMethod = order.PaymentMethod;
            existingOrder.Status = order.Status;
            existingOrder.TotalAmount = order.TotalAmount;
            existingOrder.Currency = order.Currency;

            foreach (var item in order.OrderItems)
            {
                var existingItem = existingOrder.OrderItems
                    .FirstOrDefault(x => x.ProductId == item.ProductId);

                if (existingItem is null)
                {
                    existingOrder.OrderItems.Add(new OrderItemReadModel
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,

                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice,
                        Currency = item.Currency
                    });
                }
                else
                {
                    existingItem.ProductName = item.ProductName;
                    existingItem.Quantity = item.Quantity;
                    existingItem.UnitPrice = item.UnitPrice;
                    existingItem.TotalPrice = item.TotalPrice;
                    existingItem.Currency = item.Currency;
                }
            }
        }

        await context.SaveChangesAsync();
    }

    private static OrderDto ToDto(OrderReadModel model)
    {
        return new OrderDto
        {
            Id = model.Id,
            CustomerId = model.CustomerId,
            PaymentMethod = model.PaymentMethod,
            Status = model.Status,
            TotalAmount = model.TotalAmount,
            Currency = model.Currency,
            CreatedAt = model.CreatedAt,

            OrderItems = model.OrderItems.Select(i => new OrderItemDto
            {
                Id = i.Id,
                OrderId = i.OrderId,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice,
                Currency = i.Currency
            }).ToList()
        };
    }
}