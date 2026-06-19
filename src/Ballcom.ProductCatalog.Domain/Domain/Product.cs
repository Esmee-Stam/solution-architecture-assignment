using Ballcom.ProductCatalog.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Ballcom.ProductCatalog.Domain.Domain
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Money Price { get; private set; }
        public Stock Quantity { get; private set; }

        private Product() { }

        public Product(Guid id, string name, string description, Money price, Stock quantity)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");

            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public void DecreaseStock(int amount)
        {
            Quantity = Quantity.Decrease(amount);
        }
    }
}
