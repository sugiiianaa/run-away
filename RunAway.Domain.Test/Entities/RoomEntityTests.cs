using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;
using Xunit;

namespace RunAway.Domain.Test.Entities
{
    public class RoomEntityTests
    {
        private readonly Guid _validId = Guid.NewGuid();
        private readonly string _validName = "Standard Room";
        private readonly string _validDescription = "Comfortable standard room";
        private readonly Money _validPrice = new Money(100, "USD");
        private readonly List<string> _validFacilities = new List<string> { "Free Dinner" };

        #region Creation Tests

        [Fact]
        public void Create_WithValidParameters_ReturnsRoomEntity()
        {
            // Act
            var room = RoomEntity.Create(
                _validId,
                _validName,
                _validDescription,
                _validPrice,
                _validFacilities);

            // Assert
            Assert.NotNull(room);
            Assert.Equal(_validId, room.ID);
            Assert.Equal(_validName, room.Name);
            Assert.Equal(_validDescription, room.Description);
            Assert.Equal(_validPrice, room.Price);
            Assert.Single(room.Facilities);
            Assert.Equal(_validFacilities[0], room.Facilities.First());
            Assert.Null(room.Accommodation); // Should be null before assignment
        }

        [Fact]
        public void Create_WithEmptyName_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => RoomEntity.Create(
                _validId,
                string.Empty,
                _validDescription,
                _validPrice,
                _validFacilities));

            Assert.Contains("name", exception.Message.ToLower());
        }

        [Fact]
        public void Create_WithNegativePrice_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => RoomEntity.Create(
                _validId,
                _validName,
                _validDescription,
                new Money(-100, "USD"),
                _validFacilities));

            Assert.Contains("lower than 0", exception.Message.ToLower());
        }

        [Fact]
        public void Create_WithEmptyFacilities_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => RoomEntity.Create(
                _validId,
                _validName,
                _validDescription,
                _validPrice,
                new List<string>()));

            Assert.Contains("facilities", exception.Message.ToLower());
        }

        #endregion

        #region Accommodation Assignment Tests

        [Fact]
        public void AssignToAccommodation_WithValidAccommodation_AssignsAccommodation()
        {
            // Arrange
            var room = CreateValidRoom();
            var accommodationId = Guid.NewGuid();
            var accommodationName = "Test Hotel";
            var accommodationAddress = "123 Test Street";
            var accommodationCoordinate = new Coordinate(40.7128, -74.0060);
            var accommodationImageUrls = new List<string> { "https://example.com/hotel1.jpg" };

            // We need to create the accommodation without any rooms initially to avoid circular dependency
            var accommodation = CreateAccommodation(
                accommodationId,
                accommodationName,
                accommodationAddress,
                accommodationCoordinate,
                accommodationImageUrls);

            // Act
            room.AssignToAccommodation(accommodation);

            // Assert
            Assert.NotNull(room.Accommodation);
            Assert.Equal(accommodationId, room.Accommodation.ID);

            // Verify that UpdatedAt property is updated
            Assert.NotNull(room.LastUpdatedAt);
        }

        [Fact]
        public void AssignToAccommodation_WithNullAccommodation_ThrowsArgumentNullException()
        {
            // Arrange
            var room = CreateValidRoom();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => room.AssignToAccommodation(null));
            Assert.Contains("accommodation", exception.ParamName.ToLower());
        }

        [Fact]
        public void AssignToAccommodation_WhenAlreadyAssigned_ThrowsInvalidOperationException()
        {
            // Arrange
            var room = CreateValidRoom();
            var firstAccommodation = CreateAccommodation(
                Guid.NewGuid(),
                "First Hotel",
                "123 First Street",
                new Coordinate(40.7128, -74.0060),
                new List<string> { "https://example.com/hotel1.jpg" });

            var secondAccommodation = CreateAccommodation(
                Guid.NewGuid(),
                "Second Hotel",
                "456 Second Street",
                new Coordinate(41.8781, -87.6298),
                new List<string> { "https://example.com/hotel2.jpg" });

            // Assign to first accommodation
            room.AssignToAccommodation(firstAccommodation);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                room.AssignToAccommodation(secondAccommodation));

            Assert.Contains("already assigned", exception.Message.ToLower());
        }

        #endregion

        #region Helper Methods

        private RoomEntity CreateValidRoom()
        {
            return RoomEntity.Create(
                _validId,
                _validName,
                _validDescription,
                _validPrice,
                _validFacilities);
        }

        // Helper method to create an accommodation without rooms (to avoid circular dependency)
        private AccommodationEntity CreateAccommodation(
            Guid id,
            string name,
            string address,
            Coordinate coordinate,
            List<string> imageUrls)
        {
            // We're using reflection here to create an accommodation without rooms
            // This is just for testing purposes and avoids the validation in the Create method
            var accommodationType = typeof(AccommodationEntity);
            var accommodation = (AccommodationEntity)Activator.CreateInstance(
                accommodationType,
                true);

            // Set properties using reflection
            var idProperty = accommodationType.GetProperty("Id");
            idProperty.SetValue(accommodation, id);

            var nameProperty = accommodationType.GetProperty("Name");
            nameProperty.SetValue(accommodation, name);

            var addressProperty = accommodationType.GetProperty("Address");
            addressProperty.SetValue(accommodation, address);

            var coordinateProperty = accommodationType.GetProperty("Coordinate");
            coordinateProperty.SetValue(accommodation, coordinate);

            // Add image URLs using the AddImage method
            foreach (var imageUrl in imageUrls)
            {
                var addImageMethod = accommodationType.GetMethod("AddImage");
                addImageMethod.Invoke(accommodation, new object[] { imageUrl });
            }

            return accommodation;
        }

        #endregion
    }
}
