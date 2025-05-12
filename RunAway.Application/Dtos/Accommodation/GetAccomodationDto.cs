using RunAway.Domain.Entities;

namespace RunAway.Application.Dtos.Accommodation
{
    // Response
    public class GetAccomodationDetailResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<string> ImageUrls { get; set; } = [];
        public List<GetAccommoRoomDetailResponseDto> Rooms { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class GetAccommoRoomDetailResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public required string Currency { get; set; }
        public List<string> Facilities { get; set; } = [];
    }

    // Mapper
    public static class GetAccommodationDetailMapper
    {
        public static GetAccomodationDetailResponseDto ToGetAccommodationDetailMapperDto(AccommodationEntity accommodationEntity)
        {
            return new GetAccomodationDetailResponseDto
            {
                Id = accommodationEntity.Id,
                Name = accommodationEntity.Name,
                Address = accommodationEntity.Address,
                Latitude = accommodationEntity.Coordinate.Latitude,
                Longitude = accommodationEntity.Coordinate.Longitude,
                ImageUrls = [.. accommodationEntity.ImageUrls],
                Rooms = accommodationEntity.Rooms.Select(
                    r => new GetAccommoRoomDetailResponseDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description ?? string.Empty,
                        Price = r.Price.Amount,
                        Currency = r.Price.Currency,
                        Facilities = r.Facilities.ToList()
                    }).ToList()
            };
        }
    }
}
