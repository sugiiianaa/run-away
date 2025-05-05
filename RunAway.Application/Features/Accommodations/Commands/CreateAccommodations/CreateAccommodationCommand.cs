using MediatR;
using Microsoft.Extensions.Logging;
using RunAway.Application.IRepositories;
using RunAway.Domain.Commons;
using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Features.Accommodations.Commands.CreateAccommodations
{
    public class CreateAccommodationCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public List<CreateRoomDto> Rooms { get; set; } = new();
    }

    public class CreateRoomDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public List<string> Facilities { get; set; } = new();
    }

    public class CreateAccommodationCommandHandler : IRequestHandler<CreateAccommodationCommand, Guid>
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateAccommodationCommandHandler> _logger;

        public CreateAccommodationCommandHandler(IAccommodationRepository accommodationRepository, IUnitOfWork unitOfWork, ILogger<CreateAccommodationCommandHandler> logger)
        {
            _accommodationRepository = accommodationRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var rooms = request.Rooms.Select(r => RoomEntity.Create(
                Guid.NewGuid(),
                r.Name,
                r.Description,
                new Money(r.Price, r.Currency),
                r.Facilities)).ToList();

            var coordinate = new Coordinate(request.Latitude, request.Longitude);

            var accommodation = AccommodationEntity.Create(
                Guid.NewGuid(),
                request.Name,
                request.Address,
                coordinate,
                request.ImageUrls,
                rooms);

            await _accommodationRepository.AddAsync(accommodation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Accommodation {AccommodationId} created successfully", accommodation.Id);

            return accommodation.Id;
        }
    }
}
