using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Shelters
{
    public class ShelterPetUpdated : IDomainEvent
    {
        public ShelterPet Pet { get; }

        public ShelterPetUpdated(ShelterPet pet)
        {
            Pet = pet;
        }
    }
}