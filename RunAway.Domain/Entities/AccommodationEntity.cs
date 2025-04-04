using RunAway.Domain.Commons;
using RunAway.Domain.ValueObjects;

namespace RunAway.Domain.Entities
{
    public class AccommodationEntity : AuditableEntity<Guid>
    {
        private readonly List<string> _imageUrls = [];
        private readonly List<RoomEntity> _rooms = [];

        public string Name { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public Coordinate Coordinate { get; private set; } = default!;

        public IReadOnlyCollection<string> ImageUrls => _imageUrls.AsReadOnly();
        public IReadOnlyCollection<RoomEntity> Rooms => _rooms.AsReadOnly();

        private AccommodationEntity() { } // For entity framework

        public AccommodationEntity(
            Guid id,
            string name,
            string address,
            Coordinate coordinate,
            List<string> imageUrls,
            List<RoomEntity> rooms) : base(id)
        {
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

            Id = id;
            Name = name;
            Address = address;
            Coordinate = coordinate;

            foreach (var imageUrl in imageUrls)
            {
                AddImage(imageUrl);
            }

            _rooms.AddRange(rooms);
        }

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

        public void AddRoom(RoomEntity room)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room), "Room cannot be null.");

            if (_rooms.Any(r => r.Id == room.Id))
                throw new InvalidOperationException("Room already exists in this accommodation.");

            room.AssignToAccommodation(this);

            _rooms.Add(room);
            SetUpdatedAt();
        }

        public void RemoveRoom(Guid roomId)
        {
            var room = _rooms.FirstOrDefault(r => r.Id == roomId);
            if (room != null)
            {
                _rooms.Remove(room);
                SetUpdatedAt();
            }
        }

    }
}
