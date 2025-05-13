namespace RunAway.Domain.ValueObjects
{
    public sealed class Money : IEquatable<Money>
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount shouldn't be lower than 0");

            if (string.IsNullOrEmpty(currency))
                throw new ArgumentNullException(nameof(currency), "Currency shoudln't be null");

            Amount = amount;
            Currency = currency;
        }

        public override bool Equals(object? obj)
        {
            return obj is Money other && Equals(other);
        }

        public bool Equals(Money? other)
        {
            if (other is null) return false;

            return other != null
                && Amount == other.Amount
                && Currency == other.Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
    }
}
