using System;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Shelters
{
    public class ShelterPetPhotoDeleted : IDomainEvent
    {
        public ShelterPet Pet { get; }
        public Guid DeletedPhotoId { get; }

        public ShelterPetPhotoDeleted(ShelterPet pet, Guid deletedPhotoId)
        {
            Pet = pet;
            DeletedPhotoId = deletedPhotoId;
        }
    }
}