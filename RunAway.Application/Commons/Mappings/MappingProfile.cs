using AutoMapper;
using RunAway.Application.Features.Accommodations.Queries.GetAccommodationDetails;
using RunAway.Domain.Entities;

namespace RunAway.Application.Commons.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccommodationEntity, AccommodationDetailsViewModel>()
               .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Coordinate.Latitude))
               .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Coordinate.Longitude));

            CreateMap<RoomEntity, RoomViewModel>()
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price.Amount))
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.Price.Currency));

        }
    }
}
