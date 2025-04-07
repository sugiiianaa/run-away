using RunAway.Domain.Commons;
using RunAway.Domain.Entities;

namespace RunAway.Domain.Events
{
    public class RoomAddedToAccommodationEvent : IDomainEvent
    {
        public AccommodationEntity Accommodation { get; }
        public RoomEntity Room { get; }

        public RoomAddedToAccommodationEvent(AccommodationEntity accommodation, RoomEntity room)
        {
            Accommodation = accommodation;
            Room = room;
        }
    }
}
