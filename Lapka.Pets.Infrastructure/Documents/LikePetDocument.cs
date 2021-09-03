using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Pets.Infrastructure.Documents
{
    public class LikePetDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; }
        public List<Guid> LikedPets { get; }

        public LikePetDocument(Guid id, List<Guid> likedPets)
        {
            Id = id;
            LikedPets = likedPets;
        }
    }
}