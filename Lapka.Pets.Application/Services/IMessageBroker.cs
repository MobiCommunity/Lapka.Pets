using Convey.CQRS.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lapka.Pets.Application.Services
{
    public interface IMessageBroker
    {
        Task PublishAsync(params IEvent[] events);
        Task PublishAsync(IEnumerable<IEvent> events);
    }
}