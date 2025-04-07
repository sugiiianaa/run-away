namespace RunAway.Domain.Commons
{
    public interface IDomainEventService
    {
        Task PublishAsync(IDomainEvent domainEvent);
    }
}
