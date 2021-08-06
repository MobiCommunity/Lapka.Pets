using System;
using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete;

namespace Lapka.Pets.Application.Events.Concrete
{
    public class PetUpdatedHandler : IDomainEventHandler<PetUpdated>
    {

        public Task HandleAsync(PetUpdated @event)
        {
            Console.WriteLine($"i caught {@event.Pet.Name}");
            return Task.CompletedTask;
        }
    }
}