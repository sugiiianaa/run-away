namespace RunAway.Domain.ValueObjects
{
    public sealed class Coordinate : IEquatable<Coordinate>
    {
        public decimal Latitude { get; }
        public decimal Longitude { get; }

        private Coordinate() { }

        public Coordinate(decimal latitude, decimal longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90.");

            if (longitude < -180 || longitude > 180)
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180.");

            Latitude = latitude;
            Longitude = longitude;
        }

        public override bool Equals(object? obj)
        {
            return obj is Coordinate other && Equals(other);
        }

        public bool Equals(Coordinate? other)
        {
            if (other is null) return false;

            return other != null
                && Latitude == other.Latitude
                && Longitude == other.Longitude;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude);
        }
    }
}
