using System;
using System.Collections.Generic;
using System.Linq;

namespace Lapka.Pets.Core.Entities
{
    public class Shelter : AggregateRoot
    {
        private ISet<Guid> _owners = new HashSet<Guid>();
        public IEnumerable<Guid> Owners
        {
            get => _owners;
            private set => _owners = new HashSet<Guid>(value);
        }

        public Shelter(Guid shelterId, IEnumerable<Guid> owners)
        {
            Id = new AggregateId(shelterId);
            Owners = owners ?? Enumerable.Empty<Guid>();
        }

        public static Shelter Create(Guid shelterId, IEnumerable<Guid> owners = null)
        {
            Shelter shelter = new Shelter(shelterId, owners);
            return shelter;
        }
    }
}