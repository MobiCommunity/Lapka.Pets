using System.Threading.Tasks;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Application.Events.Abstract
{
    public interface IDomainEventHandler<in T> where T : class, IDomainEvent
    {
        Task HandleAsync(T @event);
    }
}