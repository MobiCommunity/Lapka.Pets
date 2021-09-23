using System.Collections.Generic;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Mongo.Documents
{
    public class UserPetDocument : PetDocument
    {
        public IEnumerable<PetEventDocument> SoonEvents { get; set; }
        public IEnumerable<VisitDocument> Visits { get; set; }
    }
}