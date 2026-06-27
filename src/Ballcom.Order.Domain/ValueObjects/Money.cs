namespace Ballcom.Order.Domain.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");
            }
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("Currency is required.", nameof(currency));
            }
            Amount = amount;
            Currency = currency.ToUpper();
        }

        public Money Multiply(int quantity)
        {
            return new Money(Amount * quantity, Currency);
        }

        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
            {
                throw new InvalidOperationException("Cannot add Money with different currencies.");
            }

            return new Money(a.Amount + b.Amount, a.Currency);
        }
    }
}
