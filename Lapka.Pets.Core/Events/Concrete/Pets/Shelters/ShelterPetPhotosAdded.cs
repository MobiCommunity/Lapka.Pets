using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Shelters
{
    public class ShelterPetPhotosAdded : IDomainEvent
    {     
        public ShelterPet Pet { get; }
        public List<Guid> AddedPhotoIds { get; }

        public ShelterPetPhotosAdded(ShelterPet pet, List<Guid> addedPhotoIds)
        {
            Pet = pet;
            AddedPhotoIds = addedPhotoIds;
        }
    }
}