using Convey.CQRS.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Services;

namespace Lapka.Pets.Infrastructure.Services
{
    public class DummyMessageBroker : IMessageBroker
    {
        public Task PublishAsync(params IEvent[] events)
        {
            return Task.CompletedTask;
        }

        public Task PublishAsync(IEnumerable<IEvent> events)
        {
            return Task.CompletedTask;
        }
    }
}