using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Losts
{
    public class LostPetCreated : IDomainEvent
    {
        public LostPet Pet { get; }
        
        public LostPetCreated(LostPet pet)
        {
            Pet = pet;
        }

    }
}