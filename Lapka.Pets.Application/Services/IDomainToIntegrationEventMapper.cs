using Convey.CQRS.Events;
using System.Collections.Generic;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Application.Services
{
    public interface IDomainToIntegrationEventMapper
    {
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
        IEvent Map(IDomainEvent @event);
        
    }
}