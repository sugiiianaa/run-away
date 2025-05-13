using RunAway.Domain.Entities;

namespace RunAway.Application.Dtos.AccommodationAvailability
{
    // Request 
    public class CreateAccommodationAvailabilityRequestDto
    {
        required public IList<CreateAccommodationAvailabilityDto> RoomAvailability { get; set; }
    }

    public class CreateAccommodationAvailabilityDto
    {
        required public Guid RoomID { get; set; }
        required public DateOnly AvailableDate { get; set; }
        required public int AvailableRooms { get; set; }
    }

    // Response
    public class CreateAccommodationAvailabilityResponseDto
    {
        required public IList<CreateAcommodationAvailabilityResultDto> RoomAvailabilityRecords { get; set; }
    }

    public class CreateAcommodationAvailabilityResultDto
    {
        required public Guid ID { get; set; }
        required public Guid RoomID { get; set; }
        required public DateOnly AvailableDate { get; set; }
        required public int AvailableRoom { get; set; }
    }

    // Mapper
    public static class CreateAccommodationAvailabilityMapper
    {
        public static CreateAccommodationAvailabilityResponseDto ToCreateAccommodationAvailabilityResponseDto(IList<RoomAvailableRecordEntity> entity)
        {
            return new CreateAccommodationAvailabilityResponseDto
            {
                RoomAvailabilityRecords = entity.Select(
                    ra => new CreateAcommodationAvailabilityResultDto
                    {
                        ID = ra.ID,
                        RoomID = ra.RoomId,
                        AvailableDate = ra.Date,
                        AvailableRoom = ra.AvailableRooms
                    }).ToList()
            };
        }
    }
}
