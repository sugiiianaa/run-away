using MediatR;
using RunAway.Domain.Commons;

namespace RunAway.Infrastructure.Services
{
    public class DomainEventService : IDomainEventService
    {
        private readonly IMediator _mediator;

        public DomainEventService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishAsync(IDomainEvent domainEvent)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}
