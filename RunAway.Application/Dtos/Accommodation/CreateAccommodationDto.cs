using RunAway.Application.Dtos.Room;
using RunAway.Domain.Entities;

namespace RunAway.Application.Dtos.Accommodation
{
    // Response
    public class CreateAccommodationResponseDto
    {
        public Guid AccommodationId { get; set; }
        public required string Name { get; set; }
        public required IList<CreateRoomResponseDto> Rooms { get; set; }
    }

    // Mapper
    public static class CreateAccommodationMapper
    {
        public static CreateAccommodationResponseDto ToCreateAccommodationResponseDto(AccommodationEntity accommodationEntity)
        {
            return new CreateAccommodationResponseDto
            {
                AccommodationId = accommodationEntity.Id,
                Name = accommodationEntity.Name,
                Rooms = accommodationEntity.Rooms
                    .Select(room => new CreateRoomResponseDto
                    {
                        RoomId = room.Id,
                        Name = room.Name,
                        Price = room.Price
                    })
                    .ToList()
            };
        }
    }
}
