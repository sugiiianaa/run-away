namespace RunAway.Domain.ValueObjects
{
    /// <summary>
    /// Represents the availability of rooms on a specific date
    /// </summary>
    public class Availability : IEquatable<Availability>
    {
        public DateOnly Date { get; }
        public int AvailableRooms { get; }

        public Availability(DateOnly date, int availableRooms)
        {
            ValidateDate(date);
            ValidateAvailableRooms(availableRooms);

            Date = date;
            AvailableRooms = availableRooms;
        }

        private void ValidateDate(DateOnly date)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (date < today)
            {
                throw new ArgumentException("Availability date cannot be in the past", nameof(date));
            }
        }

        private void ValidateAvailableRooms(int availableRooms)
        {
            if (availableRooms < 0)
            {
                throw new ArgumentException("Available rooms cannot be negative", nameof(availableRooms));
            }
        }

        public bool HasAvailability() => AvailableRooms > 0;

        public Availability WithAvailableRooms(int newCount)
        {
            return new Availability(Date, newCount);
        }

        // Value objects must override Equals, GetHashCode, and operator == and !=
        public bool Equals(Availability? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Date.Equals(other.Date) &&
                   AvailableRooms == other.AvailableRooms;
        }

        public override bool Equals(object? obj)
        {
            return obj is Availability availability && Equals(availability);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, AvailableRooms);
        }

        public static bool operator ==(Availability? left, Availability? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;

            return left.Equals(right);
        }

        public static bool operator !=(Availability? left, Availability? right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd}: {AvailableRooms} room(s) available";
        }
    }
}
