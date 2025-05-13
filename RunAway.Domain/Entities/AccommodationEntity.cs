using System.ComponentModel.DataAnnotations;
using RunAway.Domain.Commons;
using RunAway.Domain.ValueObjects;

namespace RunAway.Domain.Entities
{
    /// <summary>
    /// Represents an acommodation in the system.
    /// Required properties: Name, Address, Coordinate, ImageUrls, Rooms
    /// </summary>
    public class AccommodationEntity : AuditableEntity<Guid>
    {
        #region Properties

        [Required]
        public string Name { get; private set; }

        [Required]
        public string Address { get; private set; }

        [Required]
        public Coordinate Coordinate { get; private set; }

        #endregion

        #region Collection Properties

        private readonly List<string> _imageUrls = [];
        private readonly List<RoomEntity> _rooms = [];

        [Required]
        public IReadOnlyCollection<string> ImageUrls => _imageUrls.AsReadOnly();

        [Required]
        public IReadOnlyCollection<RoomEntity> Rooms => _rooms.AsReadOnly();

        #endregion

        #region Constructor

        // Private parameterless constructor for EF Core
        private AccommodationEntity() : base(Guid.Empty)
        {
        }

        public static AccommodationEntity Create(
             Guid id,
             string name,
             string address,
             Coordinate coordinate,
             List<string> imageUrls,
             List<RoomEntity> rooms)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Accommodation name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Accommodation address cannot be empty.", nameof(address));

            if (coordinate == null)
                throw new ArgumentNullException(nameof(coordinate), "Coordinate is required.");

            if (imageUrls == null || imageUrls.Count == 0)
                throw new ArgumentException("At least one image is required.", nameof(imageUrls));

            if (rooms == null || rooms.Count == 0)
                throw new ArgumentException("At least one room is required.", nameof(rooms));

            var entity = new AccommodationEntity
            {
                ID = id,
                Name = name,
                Address = address,
                Coordinate = coordinate
            };

            // Add images and rooms
            foreach (var imageUrl in imageUrls)
            {
                entity._imageUrls.Add(imageUrl);
            }

            foreach (var room in rooms)
            {
                room.AssignToAccommodation(entity);
                entity._rooms.Add(room);
            }

            return entity;
        }

        #endregion

        #region Public Methods

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("New name cannot be empty.", nameof(newName));

            Name = newName;
            SetUpdatedAt();
        }

        public void UpdateAddress(string newAddress)
        {
            if (string.IsNullOrWhiteSpace(newAddress))
                throw new ArgumentException("New address cannot be empty.", nameof(newAddress));

            Address = newAddress;
            SetUpdatedAt();
        }

        public void AddImage(string imageUrl)
        {
            if (imageUrl == null)
                throw new ArgumentNullException(nameof(imageUrl), "Image URL cannot be null.");

            _imageUrls.Add(imageUrl);
            SetUpdatedAt();
        }

        public void AddRoom(IList<RoomEntity> rooms)
        {
            if (rooms == null || !rooms.Any())
                throw new ArgumentNullException(nameof(rooms), "Room cannot be null.");

            foreach (var room in rooms)
            {
                if (_rooms.Any(r => r.ID == room.ID))
                {
                    throw new InvalidOperationException("Room already exists in this accommodation.");
                }

                room.AssignToAccommodation(this);
                _rooms.Add(room);
            }
            SetUpdatedAt();
        }

        public void RemoveRoom(Guid roomId)
        {
            var room = _rooms.FirstOrDefault(r => r.ID == roomId);
            if (room != null)
            {
                _rooms.Remove(room);
                SetUpdatedAt();
            }
        }

        #endregion
    }
}
