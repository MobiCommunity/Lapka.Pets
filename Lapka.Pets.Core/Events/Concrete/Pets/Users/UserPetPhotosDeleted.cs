using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Users
{
    public class UserPetPhotosDeleted : IDomainEvent
    {
        public UserPet Pet { get; }
        public IEnumerable<string> DeletedPhotoPaths { get; }

        public UserPetPhotosDeleted(UserPet pet, IEnumerable<string> deletedPhotoPaths)
        {
            Pet = pet;
            DeletedPhotoPaths = deletedPhotoPaths;
        }
    }
}