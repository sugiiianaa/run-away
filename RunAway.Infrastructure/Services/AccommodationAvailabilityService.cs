using RunAway.Application.Commons;
using RunAway.Application.Dtos.AccommodationAvailability;
using RunAway.Application.IRepositories;
using RunAway.Application.IServices;
using RunAway.Domain.Commons;
using RunAway.Domain.Entities;

namespace RunAway.Infrastructure.Services
{
    public class AccommodationAvailabilityService : IAccommodationAvailabilityService
    {
        private readonly IAccommodationAvailabilityRepository _accommodationAvailabilityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccommodationAvailabilityService(IAccommodationAvailabilityRepository accommodationAvailabilityRepository, IUnitOfWork unitOfWork)
        {
            _accommodationAvailabilityRepository = accommodationAvailabilityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IList<RoomAvailableRecordEntity>>> AddRoomAvailabilityAsync(CreateAccommodationAvailabilityRequestDto request)
        {
            // Check if room id with same date already exists
            foreach (var roomAvailability in request.RoomAvailability)
            {
                var roomAvailabilityRecord = await _accommodationAvailabilityRepository.GetAvailabilityOnSpesificDateAsync(
                    roomAvailability.RoomID,
                    roomAvailability.AvailableDate);

                if (roomAvailabilityRecord != null)
                {
                    return Result<IList<RoomAvailableRecordEntity>>.Failure(
                        $"Availability record already exists for room {roomAvailability.RoomID} on {roomAvailability.AvailableDate}",
                        400,
                        ErrorCode.InvalidOperation);
                }
            }

            var roomAvailableRecordEntities = request.RoomAvailability.Select(
                ra => RoomAvailableRecordEntity.Create(
                    Guid.NewGuid(),
                    ra.RoomID,
                    ra.AvailableDate,
                    ra.AvailableRooms)).ToList();

            await _accommodationAvailabilityRepository.AddRoomAvailabilityAsync(roomAvailableRecordEntities);
            await _unitOfWork.SaveChangesAsync();

            return Result<IList<RoomAvailableRecordEntity>>.Success(roomAvailableRecordEntities);
        }

        public async Task<IList<RoomAvailableRecordEntity>?> GetRoomAvailabilityOnDateRangeAsync(Guid roomID, DateOnly checkInDate, DateOnly checkOutDate, int numberOfRoom)
        {
            return await _accommodationAvailabilityRepository.GetAvailabilitityOnDateRangeAsync(roomID, checkInDate, checkOutDate, numberOfRoom);
        }

        public async Task<Result<bool>> UpdateRoomAvailabilityAsync(Guid roomID, DateOnly checkInDate, DateOnly checkOutDate, int numberOfRoomsToBook)
        {
            // Get all availability records for the date range
            var availabilityRecords = await _accommodationAvailabilityRepository.GetAvailabilitityOnDateRangeAsync(
                roomID,
                checkInDate,
                checkOutDate,
                numberOfRoomsToBook);

            if (availabilityRecords == null || !availabilityRecords.Any())
            {
                return Result<bool>.Failure(
                    "No availability records found for the specified date range",
                    400,
                    ErrorCode.InvalidArgument);
            }

            // Update each record to decrease the number of available rooms
            foreach (var record in availabilityRecords)
            {
                record.DecreaseAvailableRooms(numberOfRoomsToBook);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
