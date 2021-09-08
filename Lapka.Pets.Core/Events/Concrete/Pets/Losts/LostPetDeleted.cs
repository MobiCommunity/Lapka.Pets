using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Losts
{
    public class LostPetDeleted : IDomainEvent
    {
        public LostPet Pet { get; }

        public LostPetDeleted(LostPet pet)
        {
            Pet = pet;
        }
    }
}