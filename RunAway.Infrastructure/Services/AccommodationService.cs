using RunAway.Application.Commons;
using RunAway.Application.Features.Accommodations.Commands.CreateAccommodations;
using RunAway.Application.IRepositories;
using RunAway.Application.IServices;
using RunAway.Domain.Commons;
using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;

namespace RunAway.Infrastructure.Services
{
    public class AccommodationService : IAccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccommodationService(
            IAccommodationRepository accommodationRepository,
            IUnitOfWork unitOfWork)
        {
            _accommodationRepository = accommodationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AccommodationEntity>> CreateAccommodationAsync(
            CreateAccommodationCommand command,
            CancellationToken cancellationToken)
        {
            var rooms = command.Rooms.Select(r => RoomEntity.Create(
                Guid.NewGuid(),
                r.Name,
                r.Description,
                r.Price,
                r.Facilities)).ToList();

            var coordinate = new Coordinate(command.Latitude, command.Longitude);

            var accommodation = AccommodationEntity.Create(
                Guid.NewGuid(),
                command.Name,
                command.Address,
                coordinate,
                command.ImageUrls,
                rooms
            );

            await _accommodationRepository.AddAsync(accommodation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<AccommodationEntity>.Success(accommodation);
        }

        public async Task<Result<AccommodationEntity>> GetAccommodationDetailAsync(Guid accommodationId, CancellationToken cancellationToken)
        {
            var accommodation = await _accommodationRepository.GetByIdWithRoomsAsync(accommodationId);
            if (accommodation == null)
            {
                return Result<AccommodationEntity>.Failure("Accommodation not found", 404, ErrorCode.InvalidArgument);
            }

            return Result<AccommodationEntity>.Success(accommodation);
        }
    }
}
