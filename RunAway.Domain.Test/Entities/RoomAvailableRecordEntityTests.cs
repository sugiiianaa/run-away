using RunAway.Domain.Entities;
using Xunit;

namespace RunAway.Domain.Test.Entities
{
    public class RoomAvailableRecordEntityTests
    {
        private readonly Guid _validId = Guid.NewGuid();
        private readonly Guid _validRoomId = Guid.NewGuid();
        private readonly DateOnly _validDateOnly = DateOnly.FromDateTime(DateTime.Now.AddMonths(1));
        private readonly int _validAvailableRooms = 10;

        #region Creation Tests
        // TODO : Fix this later
        //[Fact]
        //public void Create_WithValidParameters_ReturnsAvailableRecordEntity()
        //{
        //    // Act
        //    var availableRecord = RoomAvailableRecordEntity.Create(
        //        _validId,
        //        _validRoomId,
        //        _validDateOnly,
        //        _validAvailableRooms);

        //    // Assert 
        //    Assert.NotNull(availableRecord);
        //    Assert.Equal(_validId, availableRecord.Id);
        //    Assert.Equal(_validRoomId, availableRecord.RoomId);
        //    Assert.Equal(_validDateOnly, availableRecord.Date);
        //    Assert.Equal(_validAvailableRooms, availableRecord.AvailableRooms);
        //    Assert.Null(availableRecord.Room); // Should be null before assignment
        //}

        [Fact]
        public void Create_WithNegativeAvailableRoom_ThrowsArgumentException()
        {
            // Act  & Assert
            var exception = Assert.Throws<ArgumentException>(() => RoomAvailableRecordEntity.Create(
                _validId,
                _validRoomId,
                _validDateOnly,
                -1));

            Assert.Contains("negative", exception.Message.ToLower());
        }

        [Fact]
        public void Create_WithPastDate_ThrowsArgumentException()
        {
            // Arrange
            DateOnly pastDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1));

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => RoomAvailableRecordEntity.Create(
                _validId,
                _validRoomId,
                pastDate,
                _validAvailableRooms));

            Assert.Contains("past", exception.Message.ToLower());
        }

        #endregion
    }
}