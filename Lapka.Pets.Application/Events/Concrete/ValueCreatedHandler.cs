using System;
using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete;

namespace Lapka.Pets.Application.Events.Concrete
{
    public class ValueCreatedHandler : IDomainEventHandler<ValueCreated>
    {

        public Task HandleAsync(ValueCreated @event)
        {
            Console.WriteLine($"i caught {@event.Value.Name}");
            return Task.CompletedTask;
        }
    }
}