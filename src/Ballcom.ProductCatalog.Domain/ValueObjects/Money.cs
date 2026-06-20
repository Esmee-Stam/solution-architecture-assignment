namespace Ballcom.ProductCatalog.Domain.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency is required.", nameof(currency));

            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must not be negative.");

            Currency = currency;
            Amount = amount;
        }
    }
}