using RunAway.Domain.Commons;
using RunAway.Domain.ValueObjects;

namespace RunAway.Domain.Entities
{
    /// <summary>
    /// Represents a accommodation's room in the system.
    /// Required properties: Name, Price, Facilities, AccommodationId
    /// </summary>
    public class RoomEntity : AuditableEntity<Guid>
    {
        #region Properties

        public string Name { get; private set; }
        public string? Description { get; private set; }
        public Money Price { get; private set; }
        public Guid AccommodationId { get; private set; }

        #endregion

        #region Collection Properties

        private readonly List<string> _facilities = [];
        public IReadOnlyCollection<string> Facilities => _facilities.AsReadOnly();

        #endregion

        #region Navigation Properties

        public AccommodationEntity Accommodation { get; private set; }

        #endregion

        private RoomEntity() { } // For Entity Framework

        public RoomEntity(
            Guid id,
            string name,
            string description,
            Money price,
            List<string> facilities,
            Guid accommodationId) : base(id)
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
            AccommodationId = accommodationId;
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
