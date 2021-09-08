using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Shelters
{
    public class ShelterPetDeleted : IDomainEvent
    {
        public ShelterPet Pet { get; }

        public ShelterPetDeleted(ShelterPet pet)
        {
            Pet = pet;
        }
    }
}