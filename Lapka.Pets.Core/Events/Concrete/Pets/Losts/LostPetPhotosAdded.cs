using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Losts
{
    public class LostPetPhotosAdded : IDomainEvent
    {     
        public LostPet Pet { get; }
        public List<Guid> AddedPhotoIds { get; }

        public LostPetPhotosAdded(LostPet pet, List<Guid> addedPhotoIds)
        {
            Pet = pet;
            AddedPhotoIds = addedPhotoIds;
        }
    }
}