using MediatR;
using RunAway.Application.IRepositories;
using RunAway.Domain.Commons;
using RunAway.Domain.Entities;
using RunAway.Domain.Events;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Features.Accommodations.Commands.AddRoom
{
    public class AddRoomCommand : IRequest<Guid>
    {
        public Guid AccommodationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "USD";
        public List<string> Facilities { get; set; } = [];
    }

    public class AddRoomCommandHandler : IRequestHandler<AddRoomCommand, Guid>
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEventService _domainEventService;


        public AddRoomCommandHandler(IAccommodationRepository accommodationRepository, IUnitOfWork unitOfWork, IDomainEventService domainEventService)
        {
            _accommodationRepository = accommodationRepository;
            _unitOfWork = unitOfWork;
            _domainEventService = domainEventService;
        }

        public async Task<Guid> Handle(AddRoomCommand request, CancellationToken cancellationToken)
        {
            var accommodation = await _accommodationRepository.GetByIdAsync(request.AccommodationId);

            if (accommodation == null)
                throw new Exception();

            var room = new RoomEntity(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                new Money(request.Price, request.Currency),
                request.Facilities);

            accommodation.AddRoom(room);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _domainEventService.PublishAsync(new RoomAddedToAccommodationEvent(accommodation, room));

            return room.Id;
        }
    }
}
