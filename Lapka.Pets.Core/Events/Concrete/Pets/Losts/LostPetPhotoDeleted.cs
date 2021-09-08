using System;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Losts
{
    public class LostPetPhotoDeleted : IDomainEvent
    {
        public LostPet Pet { get; }
        public Guid DeletedPhotoId { get; }

        public LostPetPhotoDeleted(LostPet pet, Guid deletedPhotoId)
        {
            Pet = pet;
            DeletedPhotoId = deletedPhotoId;
        }
    }
}