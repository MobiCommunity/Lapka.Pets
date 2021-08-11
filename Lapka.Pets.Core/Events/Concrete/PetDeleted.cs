using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete
{
    public class PetDeleted : IDomainEvent
    {
        public Pet Pet { get; }

        public PetDeleted(Pet pet)
        {
            Pet = pet;
        }
    }
}