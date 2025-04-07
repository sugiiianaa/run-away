using MediatR;
using RunAway.Application.IRepositories;
using RunAway.Domain.Commons;
using RunAway.Domain.Entities;
using RunAway.Domain.Events;
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

    public class CreateAccommodationCommandHandler
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEventService _domainEventService;

        public CreateAccommodationCommandHandler(IAccommodationRepository accommodationRepository, IUnitOfWork unitOfWork, IDomainEventService domainEventService)
        {
            _accommodationRepository = accommodationRepository;
            _unitOfWork = unitOfWork;
            _domainEventService = domainEventService;
        }

        public async Task<Guid> Handle(CreateAccommodationCommand request, CancellationToken cancellationToken)
        {
            var rooms = request.Rooms.Select(r => new RoomEntity(
                Guid.NewGuid(),
                r.Name,
                r.Description,
                new Money(r.Price, r.Currency),
                r.Facilities)).ToList();

            var accommodationId = Guid.NewGuid();
            var coordinate = new Coordinate(request.Latitude, request.Longitude);

            var accommodation = new AccommodationEntity(
                accommodationId,
                request.Name,
                request.Address,
                coordinate,
                request.ImageUrls,
                rooms);

            await _accommodationRepository.AddAsync(accommodation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _domainEventService.PublishAsync(new AccommodationCreatedEvent(accommodation));

            return accommodation.Id;
        }
    }
}
