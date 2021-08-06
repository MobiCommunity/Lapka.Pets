using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete
{
    public class PetUpdated : IDomainEvent
    {
        public Pet Pet { get; }

        public PetUpdated(Pet pet)
        {
            Pet = pet;
        }
    }
}