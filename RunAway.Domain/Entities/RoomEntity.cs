using RunAway.Domain.Commons;
using RunAway.Domain.ValueObjects;

namespace RunAway.Domain.Entities
{
    public class RoomEntity : AuditableEntity<Guid>
    {
        private readonly List<string> _facilities = [];

        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public Money Price { get; private set; } = default!;

        public IReadOnlyCollection<string> Facilities => _facilities.AsReadOnly();

        // Foreign key to enforce one-to-many relationship
        public Guid AccommodationId { get; private set; }
        public AccommodationEntity Accommodation { get; private set; } = default!;

        private RoomEntity() { } // For Entity Framework

        public RoomEntity(
            Guid id,
            string name,
            string description,
            Money price,
            List<string> facilities) : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Room name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Room description cannot be empty.", nameof(description));

            if (facilities == null || facilities.Count == 0)
                throw new ArgumentException("At least one of facility is required.", nameof(facilities));

            Id = id;
            Name = name;
            Description = description;
            Price = price;
            _facilities.AddRange(facilities);
        }

        public void AssignToAccommodation(AccommodationEntity accommodation)
        {
            if (accommodation is null)
                throw new ArgumentNullException(nameof(accommodation), "Accommodation cannot be null.");

            AccommodationId = accommodation.Id;
            Accommodation = accommodation;
        }
    }
}
