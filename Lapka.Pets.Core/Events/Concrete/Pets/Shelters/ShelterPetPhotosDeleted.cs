using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Shelters
{
    public class ShelterPetPhotosDeleted : IDomainEvent
    {
        public ShelterPet Pet { get; }
        public IEnumerable<string> DeletedPhotoPaths { get; }

        public ShelterPetPhotosDeleted(ShelterPet pet, IEnumerable<string> deletedPhotoPaths)
        {
            Pet = pet;
            DeletedPhotoPaths = deletedPhotoPaths;
        }
    }
}