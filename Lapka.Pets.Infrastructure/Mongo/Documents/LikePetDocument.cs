using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Pets.Infrastructure.Mongo.Documents
{
    public class LikePetDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; }
        public IEnumerable<Guid> LikedPets { get; }

        public LikePetDocument(Guid id, IEnumerable<Guid> likedPets)
        {
            Id = id;
            LikedPets = likedPets;
        }
    }
}