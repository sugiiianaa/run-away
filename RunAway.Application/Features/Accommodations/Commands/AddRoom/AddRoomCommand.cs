using MediatR;
using Microsoft.Extensions.Logging;
using RunAway.Application.IRepositories;
using RunAway.Domain.Commons;
using RunAway.Domain.Entities;
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
        private readonly ILogger<AddRoomCommandHandler> _logger;
        private readonly IRoomRepository _roomRepository;

        public AddRoomCommandHandler(IAccommodationRepository accommodationRepository, IUnitOfWork unitOfWork, ILogger<AddRoomCommandHandler> logger, IRoomRepository roomRepository)
        {
            _accommodationRepository = accommodationRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _roomRepository = roomRepository;
        }

        public async Task<Guid> Handle(AddRoomCommand request, CancellationToken cancellationToken)
        {
            var accommodation = await _accommodationRepository.GetByIdAsync(request.AccommodationId);

            if (accommodation == null)
                throw new Exception();

            var room = RoomEntity.Create(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                new Money(request.Price, request.Currency),
                request.Facilities,
                request.AccommodationId);

            accommodation.AddRoom(room);

            await _roomRepository.AddAsync(room);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Room {RoomId} added successfully", room.Id);

            return room.Id;
        }

    }
}
