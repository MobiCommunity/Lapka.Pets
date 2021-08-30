using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete
{
    public class PetPhotoDeleted<T> : IDomainEvent where T : AggregatePet 
    {
        public T Pet { get; }
        public Guid DeletedPhotoId { get; }

        public PetPhotoDeleted(T pet, Guid deletedPhotoId)
        {
            Pet = pet;
            DeletedPhotoId = deletedPhotoId;
        }
    }
}