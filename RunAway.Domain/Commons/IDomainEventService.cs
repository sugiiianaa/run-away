namespace RunAway.Domain.Commons
{
    public interface IDomainEventService
    {
        public Task PublishAsync(IDomainEvent domainEvent);
    }
}
