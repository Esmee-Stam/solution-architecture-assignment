using Ballcom.Order.Domain.Domain;

namespace Ballcom.Order.Application.DTOs;

public record CartItemDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal PriceAmount,       
    decimal TotalPriceAmount,  
    string Currency            
)
{
    public static CartItemDto FromDomain(CartItem item)
        => new(
            item.ProductId,
            item.ProductName,
            item.Quantity,
            item.Price.Amount,
            item.TotalPrice.Amount,
            item.Price.Currency);
}
