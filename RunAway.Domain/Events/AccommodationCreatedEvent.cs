using RunAway.Domain.Commons;
using RunAway.Domain.Entities;

namespace RunAway.Domain.Events
{
    public class AccommodationCreatedEvent : IDomainEvent
    {
        public AccommodationEntity Accommodation { get; }

        public AccommodationCreatedEvent(AccommodationEntity accommodation)
        {
            Accommodation = accommodation;
        }
    }
}
