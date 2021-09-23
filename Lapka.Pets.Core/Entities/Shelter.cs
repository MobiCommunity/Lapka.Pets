using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class Shelter : AggregateRoot
    {
        private ISet<Guid> _owners = new HashSet<Guid>();
        public string Name { get; private set; }
        public Location GeoLocation { get; }
        public Address Address { get; }
        public bool IsDeleted { get; private set;}

        public IEnumerable<Guid> Owners
        {
            get => _owners;
            private set => _owners = new HashSet<Guid>(value);
        }

        public Shelter(Guid shelterId, string name, Address address, Location geoLocation, bool isDeleted,
            IEnumerable<Guid> owners = null)
        {
            Id = new AggregateId(shelterId);
            Name = name;
            Address = address;
            GeoLocation = geoLocation;
            IsDeleted = isDeleted;
            Owners = owners ?? Enumerable.Empty<Guid>();
        }

        public static Shelter Create(Guid shelterId, string name, Address shelterAddress, Location geoLocation,
            IEnumerable<Guid> owners = null)
        {
            Shelter shelter = new Shelter(shelterId, name, shelterAddress, geoLocation, false, owners);
            return shelter;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void AddOwner(Guid userId)
        {
            _owners.Add(userId);
        }

        public void RemoveOwner(Guid userId)
        {
            _owners.Remove(userId);
        }
    }
}