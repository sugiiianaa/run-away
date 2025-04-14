using System.Text.RegularExpressions;

namespace RunAway.Domain.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !IsValidEmail(value))
                throw new ArgumentException(nameof(value), "Email address isn't valid.");

            Value = value.Trim().ToLowerInvariant();
        }

        public override bool Equals(object? obj)
        {
            return obj is Email other && Equals(other);
        }

        public bool Equals(Email? other)
        {
            if (other is null) return false;

            return other != null
                && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        // Convert to/from string
        public override string ToString() => Value;
        public static implicit operator string(Email email) => email.Value;

        private static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

    }
}
