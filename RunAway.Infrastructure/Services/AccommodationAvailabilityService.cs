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

        public async Task<IList<RoomAvailableRecordEntity>?> AddRoomAvailability(CreateAccommodationAvailabilityRequestDto request)
        {
            // check if room id with same date already exist
            foreach (var roomAvailability in request.RoomAvailability)
            {
                var RoomAvailabilityRecord = await _accommodationAvailabilityRepository.GetAvailabilityOnSpesificDateAsync(roomAvailability.RoomID, roomAvailability.AvailableDate);

                // TODO : return proper error 
                if (RoomAvailabilityRecord != null)
                    return null;
            }


            var roomAvailableRecordEntities = request.RoomAvailability.Select(
                ra => RoomAvailableRecordEntity.Create(
                    Guid.NewGuid(),
                    ra.RoomID,
                    ra.AvailableDate,
                    ra.AvailableRooms)).ToList();

            await _accommodationAvailabilityRepository.AddRoomAvailability(roomAvailableRecordEntities);
            await _unitOfWork.SaveChangesAsync();

            return roomAvailableRecordEntities;
        }
    }
}
