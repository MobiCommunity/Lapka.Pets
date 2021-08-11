using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete
{
    public class PetCreated : IDomainEvent
    {
        public Pet Pet { get; }
        
        public PetCreated(Pet pet)
        {
            Pet = pet;
        }

    }
}