using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Users
{
    public class UserPetPhotosAdded : IDomainEvent
    {     
        public UserPet Pet { get; }
        public List<Guid> AddedPhotoIds { get; }

        public UserPetPhotosAdded(UserPet pet, List<Guid> addedPhotoIds)
        {
            Pet = pet;
            AddedPhotoIds = addedPhotoIds;
        }
    }
}