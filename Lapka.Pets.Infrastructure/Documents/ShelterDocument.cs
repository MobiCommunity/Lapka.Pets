using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Pets.Infrastructure.Documents
{
    public class ShelterDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> Owners { get; set; }
    }
}