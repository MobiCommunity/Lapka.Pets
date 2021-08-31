using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete
{
    public class PetPhotosAdded<T> : IDomainEvent where T : AggregatePet
    {     
        public T Pet { get; }
        public List<Guid> AddedPhotoIds { get; }

        public PetPhotosAdded(T pet, List<Guid> addedPhotoIds)
        {
            Pet = pet;
            AddedPhotoIds = addedPhotoIds;
        }
    }
}