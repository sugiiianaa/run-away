using RunAway.Application.Features.Accommodations.Commands.AddRoom;
using RunAway.Application.IRepositories;
using RunAway.Application.IServices;
using RunAway.Domain.Commons;
using RunAway.Domain.Entities;

namespace RunAway.Infrastructure.Services
{
    public class RoomService : IRoomService
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IAccommodationRepository accommodationRepository, IRoomRepository roomRepository, IUnitOfWork unitOfWork)
        {
            _accommodationRepository = accommodationRepository;
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<RoomEntity>> AddRoomAsync(
            AddRoomCommand command,
            CancellationToken cancellationToken)
        {
            var accommodation = await _accommodationRepository.GetByIdAsync(command.AccommodationId)
              ?? throw new InvalidOperationException("Cannot add room to non existent accommodation");

            var rooms = command.Rooms.Select(
                r => RoomEntity.Create(
                    Guid.NewGuid(),
                    r.Name,
                    r.Description!,
                    r.Price,
                    r.Facilities)).ToList();

            accommodation.AddRoom(rooms);

            await _roomRepository.AddAsync(rooms);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return rooms;
        }
    }
}
