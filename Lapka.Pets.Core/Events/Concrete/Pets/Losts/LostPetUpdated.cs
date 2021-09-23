using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Losts
{
    public class LostPetUpdated : IDomainEvent
    {
        public LostPet Pet { get; }

        public LostPetUpdated(LostPet pet)
        {
            Pet = pet;
        }
    }
}