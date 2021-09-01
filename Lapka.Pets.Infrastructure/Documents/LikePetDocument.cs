using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Pets.Infrastructure.Documents
{
    public class LikePetDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> LikedPets { get; set; }
    }
}