using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Shelters
{
    public class ShelterPetCreated : IDomainEvent
    {
        public ShelterPet Pet { get; }
        
        public ShelterPetCreated(ShelterPet pet)
        {
            Pet = pet;
        }

    }
}