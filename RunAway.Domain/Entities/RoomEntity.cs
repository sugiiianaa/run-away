using System.ComponentModel.DataAnnotations;
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

        [Required]
        public string Name { get; private set; }
        public string? Description { get; private set; }
        [Required]
        public Money Price { get; private set; }


        public Guid AccommodationId { get; private set; }

        #endregion

        #region Collection Properties

        private readonly List<string> _facilities = [];

        [Required]
        public IReadOnlyCollection<string> Facilities => _facilities.AsReadOnly();

        private readonly List<RoomAvailableRecordEntity> _roomAvailabilityRecords = [];
        public IReadOnlyCollection<RoomAvailableRecordEntity> RoomAvailabilityRecords => _roomAvailabilityRecords.AsReadOnly();
        #endregion

        #region Navigation Properties

        public AccommodationEntity Accommodation { get; private set; }

        #endregion

        // Private constructor for EF Core
        private RoomEntity() : base(Guid.Empty)
        {
            // Initialize collections to prevent null reference exceptions
            _facilities = new List<string>();
            _roomAvailabilityRecords = new List<RoomAvailableRecordEntity>();
        }

        public static RoomEntity Create(
            Guid id,
            string name,
            string description,
            Money price,
            List<string> facilities)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Room name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Room description cannot be empty.", nameof(description));

            if (facilities == null || facilities.Count == 0)
                throw new ArgumentException("At least one of facility is required.", nameof(facilities));

            var entity = new RoomEntity
            {

                Id = id,
                Name = name,
                Description = description,
                Price = price,
            };

            foreach (var facility in facilities)
            {
                entity._facilities.Add(facility);
            }

            return entity;
        }

        public void AssignToAccommodation(AccommodationEntity accommodation)
        {
            if (accommodation is null)
                throw new ArgumentNullException(nameof(accommodation), "Accommodation cannot be null.");

            AccommodationId = accommodation.Id;
            Accommodation = accommodation;
            SetUpdatedAt();
        }

        public void UpdateDetails(string name, string description, Money price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Room name cannot be empty.", nameof(name));

            if (price == null)
                throw new ArgumentNullException(nameof(price), "Price cannot be null.");

            Name = name;
            Description = description ?? string.Empty; // Handle null description
            Price = price;
            SetUpdatedAt();
        }

        public void AddFacility(string facility)
        {
            if (string.IsNullOrWhiteSpace(facility))
                throw new ArgumentException("Facility cannot be empty.", nameof(facility));

            if (!_facilities.Contains(facility))
            {
                _facilities.Add(facility);
                SetUpdatedAt();
            }
        }

        public void RemoveFacility(string facility)
        {
            if (_facilities.Remove(facility))
            {
                SetUpdatedAt();
            }
        }

        #region Availability Management

        /// <summary>
        /// Sets the availability for a specific date
        /// </summary>
        public void UpdateAvailability(DateOnly date, int availableRooms)
        {
            if (availableRooms < 0)
                throw new ArgumentException("Available rooms cannot be negative", nameof(availableRooms));

            var existing = _roomAvailabilityRecords.FirstOrDefault(a => a.Date == date);

            if (existing != null)
            {
                existing.UpdateAvailableRooms(availableRooms);
            }
            else
            {
                var newRecord = RoomAvailableRecordEntity.Create(Guid.NewGuid(), Id, date, availableRooms);
                _roomAvailabilityRecords.Add(newRecord);
            }

            SetUpdatedAt();
        }

        /// <summary>
        /// Sets availability for a range of dates
        /// </summary>
        public void UpdateAvailabilityRange(DateOnly startDate, DateOnly endDate, int availableRooms)
        {
            if (endDate < startDate)
                throw new ArgumentException("End date must be after start date", nameof(endDate));

            var currentDate = startDate;
            while (currentDate <= endDate)
            {
                UpdateAvailability(currentDate, availableRooms);
                currentDate = currentDate.AddDays(1);
            }

            SetUpdatedAt();
        }

        /// <summary>
        /// Check if the room is available on a specific date
        /// </summary>
        public bool IsAvailableOn(DateOnly date)
        {
            var record = _roomAvailabilityRecords.FirstOrDefault(a => a.Date == date);
            return record?.AvailableRooms > 0;
        }

        /// <summary>
        /// Gets the number of available rooms on a specific date
        /// </summary>
        public int GetAvailableRoomsOn(DateOnly date)
        {
            var record = _roomAvailabilityRecords.FirstOrDefault(a => a.Date == date);
            return record?.AvailableRooms ?? 0;
        }

        /// <summary>
        /// Gets all availabilities within a date range
        /// </summary>
        public IReadOnlyCollection<Availability> GetAvailabilitiesBetween(DateOnly startDate, DateOnly endDate)
        {
            return _roomAvailabilityRecords
                .Where(a => a.Date >= startDate && a.Date <= endDate)
                .Select(a => new Availability(a.Date, a.AvailableRooms))
                .ToList()
                .AsReadOnly();
        }

        /// <summary>
        /// Internal method used by EF Core to add availability records
        /// </summary>
        internal void AddAvailabilityRecord(RoomAvailableRecordEntity record)
        {
            if (record.RoomId != Id)
                throw new ArgumentException("Availability record must belong to this room", nameof(record));

            _roomAvailabilityRecords.Add(record);
        }
        #endregion
    }
}
