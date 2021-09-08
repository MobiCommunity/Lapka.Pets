using System;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Users
{
    public class UserPetPhotoDeleted : IDomainEvent
    {
        public UserPet Pet { get; }
        public Guid DeletedPhotoId { get; }

        public UserPetPhotoDeleted(UserPet pet, Guid deletedPhotoId)
        {
            Pet = pet;
            DeletedPhotoId = deletedPhotoId;
        }
    }
}