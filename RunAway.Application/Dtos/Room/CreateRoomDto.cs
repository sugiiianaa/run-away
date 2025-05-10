using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Dtos.Room
{
    // Request
    public class CreateRoomRequestDto
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
        public required Money Price { get; init; }
        public required List<string> Facilities { get; init; } = [];
    }

    // Response
    public class CreateRoomResponseDto
    {
        public Guid RoomId { get; init; }
        public string Name { get; init; }
        public Money Price { get; init; }
    }

    // Mapper
    public static class CreateRoomMapper
    {
        public static IList<CreateRoomResponseDto> ToCreateRoomResponseDto(IList<RoomEntity> roomEntity)
        {
            return roomEntity.Select(
                r => new CreateRoomResponseDto
                {
                    RoomId = r.AccommodationId,
                    Name = r.Name,
                    Price = r.Price,
                }).ToList();
        }
    }
}
