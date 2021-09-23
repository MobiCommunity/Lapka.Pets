using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Losts
{
    public class LostPetPhotosDeleted : IDomainEvent
    {
        public LostPet Pet { get; }
        public IEnumerable<string> DeletedPhotoPaths { get; }

        public LostPetPhotosDeleted(LostPet pet, IEnumerable<string> deletedPhotoPaths)
        {
            Pet = pet;
            DeletedPhotoPaths = deletedPhotoPaths;
        }
    }
}