using System.Collections.Generic;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Infrastructure.Documents
{
    public class PetUserDocument : PetDocument
    {
        public IEnumerable<PetEventDocument> SoonEvents { get; set; }
        public IEnumerable<VisitDocument> Visits { get; set; }
    }
}