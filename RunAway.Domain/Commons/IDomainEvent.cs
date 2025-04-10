using MediatR;

namespace RunAway.Domain.Commons
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}
