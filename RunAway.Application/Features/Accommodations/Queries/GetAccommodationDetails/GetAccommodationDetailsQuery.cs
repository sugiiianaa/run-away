using AutoMapper;
using MediatR;
using RunAway.Application.IRepositories;

namespace RunAway.Application.Features.Accommodations.Queries.GetAccommodationDetails
{
    public class GetAccommodationDetailsQuery : IRequest<AccommodationDetailsViewModel>
    {
        public Guid Id { get; set; }
    }

    public class AccommodationDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public List<RoomViewModel> Rooms { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class RoomViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public List<string> Facilities { get; set; } = new();
    }

    public class GetAccommodationDetailsQueryHandler : IRequestHandler<GetAccommodationDetailsQuery, AccommodationDetailsViewModel>
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IMapper _mapper;

        public GetAccommodationDetailsQueryHandler(IAccommodationRepository accommodationRepository, IMapper mapper)
        {
            _accommodationRepository = accommodationRepository;
            _mapper = mapper;
        }

        public async Task<AccommodationDetailsViewModel> Handle(GetAccommodationDetailsQuery request, CancellationToken cancellationToken)
        {
            var accommodation = await _accommodationRepository.GetByIdWithRoomsAsync(request.Id);

            if (accommodation == null)
                throw new Exception();

            return _mapper.Map<AccommodationDetailsViewModel>(accommodation);
        }
    }
}
