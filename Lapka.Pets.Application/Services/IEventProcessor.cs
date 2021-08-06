using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Application.Services
{
    public interface IEventProcessor
    {
        Task ProcessAsync(IEnumerable<IDomainEvent> events);
    }
}