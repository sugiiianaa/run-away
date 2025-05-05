using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;
using Xunit;

namespace RunAway.Domain.Test.Entities
{
    public class AccommodationEntityTests
    {
        private readonly Guid _validId = Guid.NewGuid();
        private readonly string _validName = "Test Hotel";
        private readonly string _validAddress = "123 Test Street, Test City";
        private readonly Coordinate _validCoordinate = new Coordinate(40.7128, -74.0060);
        private readonly List<string> _validImageUrls = new List<string> { "https://example.com/image1.jpg" };
        private readonly List<RoomEntity> _validRooms;

        public AccommodationEntityTests()
        {
            // Setup valid rooms for reuse across tests
            _validRooms = new List<RoomEntity>
            {
                RoomEntity.Create(
                    Guid.NewGuid(),
                    "Standard Room",
                    "Comfortable standard room",
                    new Money(100, "USD"),
                    new List<string> { "Free Dinner"}
                    )
            };
        }

        #region Creation Tests

        [Fact]
        public void Create_WithValidParameters_ReturnsAccommodationEntity()
        {
            // Act
            var accommodation = AccommodationEntity.Create(
                _validId,
                _validName,
                _validAddress,
                _validCoordinate,
                _validImageUrls,
                _validRooms);

            // Assert
            Assert.NotNull(accommodation);
            Assert.Equal(_validId, accommodation.Id);
            Assert.Equal(_validName, accommodation.Name);
            Assert.Equal(_validAddress, accommodation.Address);
            Assert.Equal(_validCoordinate, accommodation.Coordinate);
            Assert.Single(accommodation.ImageUrls);
            Assert.Equal(_validImageUrls[0], accommodation.ImageUrls.First());
            Assert.Single(accommodation.Rooms);
        }

        [Fact]
        public void Create_WithEmptyName_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => AccommodationEntity.Create(
                _validId,
                string.Empty,
                _validAddress,
                _validCoordinate,
                _validImageUrls,
                _validRooms));

            Assert.Equal("Accommodation name cannot be empty. (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void Create_WithEmptyAddress_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => AccommodationEntity.Create(
                _validId,
                _validName,
                string.Empty,
                _validCoordinate,
                _validImageUrls,
                _validRooms));

            Assert.Equal("Accommodation address cannot be empty. (Parameter 'address')", exception.Message);
        }

        [Fact]
        public void Create_WithNullCoordinate_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => AccommodationEntity.Create(
                _validId,
                _validName,
                _validAddress,
                null,
                _validImageUrls,
                _validRooms));

            Assert.Equal("Coordinate is required. (Parameter 'coordinate')", exception.Message);
        }

        [Fact]
        public void Create_WithEmptyImageUrls_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => AccommodationEntity.Create(
                _validId,
                _validName,
                _validAddress,
                _validCoordinate,
                new List<string>(),
                _validRooms));

            Assert.Equal("At least one image is required. (Parameter 'imageUrls')", exception.Message);
        }

        [Fact]
        public void Create_WithNullImageUrls_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => AccommodationEntity.Create(
                _validId,
                _validName,
                _validAddress,
                _validCoordinate,
                null,
                _validRooms));

            Assert.Equal("At least one image is required. (Parameter 'imageUrls')", exception.Message);
        }

        [Fact]
        public void Create_WithEmptyRooms_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => AccommodationEntity.Create(
                _validId,
                _validName,
                _validAddress,
                _validCoordinate,
                _validImageUrls,
                new List<RoomEntity>()));

            Assert.Equal("At least one room is required. (Parameter 'rooms')", exception.Message);
        }

        [Fact]
        public void Create_WithNullRooms_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => AccommodationEntity.Create(
                _validId,
                _validName,
                _validAddress,
                _validCoordinate,
                _validImageUrls,
                null));

            Assert.Equal("At least one room is required. (Parameter 'rooms')", exception.Message);
        }

        #endregion

        #region Method Tests

        [Fact]
        public void UpdateName_WithValidName_UpdatesName()
        {
            // Arrange
            var accommodation = CreateValidAccommodation();
            var newName = "Updated Hotel Name";

            // Act
            accommodation.UpdateName(newName);

            // Assert
            Assert.Equal(newName, accommodation.Name);
            // Verify that UpdatedAt property is updated
            Assert.NotNull(accommodation.LastUpdatedAt);
        }

        [Fact]
        public void UpdateName_WithEmptyName_ThrowsArgumentException()
        {
            // Arrange
            var accommodation = CreateValidAccommodation();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => accommodation.UpdateName(string.Empty));
            Assert.Equal("New name cannot be empty. (Parameter 'newName')", exception.Message);
        }

        [Fact]
        public void UpdateAddress_WithValidAddress_UpdatesAddress()
        {
            // Arrange
            var accommodation = CreateValidAccommodation();
            var newAddress = "456 New Street, New City";

            // Act
            accommodation.UpdateAddress(newAddress);

            // Assert
            Assert.Equal(newAddress, accommodation.Address);
            // Verify that UpdatedAt property is updated
            Assert.NotNull(accommodation.LastUpdatedAt);
        }

        [Fact]
        public void UpdateAddress_WithEmptyAddress_ThrowsArgumentException()
        {
            // Arrange
            var accommodation = CreateValidAccommodation();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => accommodation.UpdateAddress(string.Empty));
            Assert.Equal("New address cannot be empty. (Parameter 'newAddress')", exception.Message);
        }

        [Fact]
        public void AddImage_WithValidUrl_AddsImage()
        {
            // Arrange
            var accommodation = CreateValidAccommodation();
            var newImageUrl = "https://example.com/image2.jpg";
            var initialImageCount = accommodation.ImageUrls.Count;

            // Act
            accommodation.AddImage(newImageUrl);

            // Assert
            Assert.Equal(initialImageCount + 1, accommodation.ImageUrls.Count);
            Assert.Contains(newImageUrl, accommodation.ImageUrls);
            // Verify that UpdatedAt property is updated
            Assert.NotNull(accommodation.LastUpdatedAt);
        }

        [Fact]
        public void AddImage_WithNullUrl_ThrowsArgumentNullException()
        {
            // Arrange
            var accommodation = CreateValidAccommodation();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => accommodation.AddImage(null));
            Assert.Equal("Image URL cannot be null. (Parameter 'imageUrl')", exception.Message);
        }

        [Fact]
        public void AddRoom_WithValidRoom_AddsRoom()
        {
            // Arrange
            var accommodation = CreateValidAccommodation();
            var newRoom = RoomEntity.Create(
                    Guid.NewGuid(),
                    "Standard Room",
                    "Comfortable standard room",
                    new Money(100, "USD"),
                    new List<string> { "Free Dinner" }
                    );
            var initialRoomCount = accommodation.Rooms.Count;

            // Act
            accommodation.AddRoom(newRoom);

            // Assert
            Assert.Equal(initialRoomCount + 1, accommodation.Rooms.Count);
            Assert.Contains(newRoom, accommodation.Rooms);
            // Verify that UpdatedAt property is updated
            Assert.NotNull(accommodation.LastUpdatedAt);
        }

        [Fact]
        public void AddRoom_WithNullRoom_ThrowsArgumentNullException()
        {
            // Arrange
            var accommodation = CreateValidAccommodation();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => accommodation.AddRoom(null));
            Assert.Equal("Room cannot be null. (Parameter 'room')", exception.Message);
        }

        [Fact]
        public void AddRoom_WithExistingRoomId_ThrowsInvalidOperationException()
        {
            // Arrange
            var accommodation = CreateValidAccommodation();
            var existingRoomId = accommodation.Rooms.First().Id;

            // Create a room with the same ID as an existing room
            var duplicateRoom = RoomEntity.Create(
                existingRoomId,
                "Duplicate Room",
                "This has a duplicate ID",
                new Money(100, "USD"),
                new List<string> { "Free Dinner" });

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => accommodation.AddRoom(duplicateRoom));
            Assert.Equal("Room already exists in this accommodation.", exception.Message);
        }

        [Fact]
        public void RemoveRoom_WithExistingRoomId_RemovesRoom()
        {
            // Arrange
            var accommodation = CreateValidAccommodation();
            var roomToRemove = accommodation.Rooms.First();
            var initialRoomCount = accommodation.Rooms.Count;

            // Act
            accommodation.RemoveRoom(roomToRemove.Id);

            // Assert
            Assert.Equal(initialRoomCount - 1, accommodation.Rooms.Count);
            Assert.DoesNotContain(roomToRemove, accommodation.Rooms);
            // Verify that UpdatedAt property is updated
            Assert.NotNull(accommodation.LastUpdatedAt);
        }

        [Fact]
        public void RemoveRoom_WithNonExistentRoomId_DoesNotModifyRooms()
        {
            // Arrange
            var accommodation = CreateValidAccommodation();
            var initialRoomCount = accommodation.Rooms.Count;
            var nonExistentRoomId = Guid.NewGuid();

            // Act
            accommodation.RemoveRoom(nonExistentRoomId);

            // Assert
            Assert.Equal(initialRoomCount, accommodation.Rooms.Count);
        }

        #endregion

        #region Helper Methods

        private AccommodationEntity CreateValidAccommodation()
        {
            return AccommodationEntity.Create(
                _validId,
                _validName,
                _validAddress,
                _validCoordinate,
                _validImageUrls,
                _validRooms);
        }

        #endregion
    }
}
