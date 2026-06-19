namespace Ballcom.ProductCatalog.Domain.ValueObjects
{
    public record Stock
    {
        public int Value { get; init; }

        public Stock(int value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Stock can not be negative.");

            Value = value;
        }

        public Stock Decrease(int amount)
        {
            if (amount < 0) throw new ArgumentException("Amount must be positive.", nameof(amount));

            if (Value - amount < 0) throw new InvalidOperationException("Stock cannot go below zero.");

            return new Stock(Value - amount);
        }
    }
}