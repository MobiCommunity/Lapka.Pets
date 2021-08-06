using System;
using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete;

namespace Lapka.Pets.Application.Events.Concrete
{
    public class PetCreatedHandler : IDomainEventHandler<PetCreated>
    {
        public Task HandleAsync(PetCreated @event)
        {
            Console.WriteLine($"i caught {@event.Pet.Name}");
            return Task.CompletedTask;
        }
    }
}