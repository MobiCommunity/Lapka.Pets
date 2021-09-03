using System;
using System.Collections.Generic;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Infrastructure.Documents
{
    public class UserPetDocument : PetDocument
    {
        public Guid UserId { get; set; }
        public IEnumerable<PetEventDocument> SoonEvents { get; set; }
        public IEnumerable<VisitDocument> Visits { get; set; }
    }
}