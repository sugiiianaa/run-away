using RunAway.Domain.Enums;

namespace RunAway.Domain.ValueObjects
{
    public class Guest
    {
        public GuestType Type { get; set; }
        public int Number { get; set; }

        private Guest() { }

        public Guest(GuestType type, int number)
        {
            Type = type;
            Number = number;
        }

        public override bool Equals(object? obj)
        {
            return obj is Guest && Equals((Guest)obj);
        }

        public bool Equals(Guest other)
        {
            if (other == null) return false;

            return other != null
                && Type == other.Type
                && Number == other.Number;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Number);
        }
    }
}
