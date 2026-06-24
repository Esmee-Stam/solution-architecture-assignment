namespace Ballcom.Order.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required.", nameof(currency));

        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must not be negative.");

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static Money Zero(string currency)
    {
        return new Money(0, currency);
    }

    public Money Multiply(int factor)
    {
        if (factor < 0)
            throw new ArgumentOutOfRangeException(nameof(factor), "Factor must not be negative.");

        return new Money(Amount * factor, Currency);
    }

    public Money Add(Money other)
    {
        if (other is null) throw new ArgumentNullException(nameof(other));

        if (!string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Money values must use the same currency.");

        return new Money(Amount + other.Amount, Currency);
    }
}
