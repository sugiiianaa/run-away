using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Dtos.Accommodation
{
    // Request 
    public class CreateAccommodationRoomRequestDto
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
        public required Money Price { get; init; }
        public required List<string> Facilities { get; init; } = [];
    }


    // Response
    public class CreateAccommodationResponseDto
    {
        public Guid AccommodationId { get; set; }
        public required string Name { get; set; }
        public required IList<CreateAccommodationRoomResponse> Rooms { get; set; }
    }

    public class CreateAccommodationRoomResponse
    {
        public Guid RoomId { get; init; }
        public required string Name { get; init; }
        public required Money Price { get; init; }
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
                    .Select(room => new CreateAccommodationRoomResponse
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
