using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.ValueObject;

namespace Lapka.Pets.Core.Entities
{
    public abstract class Pet : AggregateRoot
    {
        public string Name { get; private set; }
        public string Race { get; private set; }
        public Sex Sex { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string Description { get; private set; }
        public Location Geolocation { get; private set; }
        public ShelterAddress? ShelterAddress { get; private set; }
        public bool? Sterilization { get; private set; }
        public double Weight { get; private set; }
        public string Color { get; private set; }
        public Species Species { get; }

        public Pet(Guid id, string name, string race, Sex sex, DateTime dateOfBirth, string description, Location geolocation, 
            ShelterAddress shelterAddress, bool sterilization, bool isLiked, double weight, string color, Species species)
        {
            Id = new AggregateId(id);
            Name = name;
            Race = race;
            Sex = sex;
            DateOfBirth = dateOfBirth;
            Description = description;
            Geolocation = geolocation;
            ShelterAddress = shelterAddress;
            Sterilization = sterilization;
            Weight = weight;
            Color = color;
            Species = species;
        }

        public void Delete()
        {
            AddEvent(new PetDeleted(this));
        }

        public void Update(string name, string race, Sex sex, DateTime dateOfBirth, string description,
            Location geoLocation, ShelterAddress shelterAddress, bool sterilization, bool isLiked, double weight,
            string color)
        {
            Name = name;
            Race = race;
            Sex = sex;
            DateOfBirth = dateOfBirth;
            Description = description;
            Geolocation = geoLocation;
            ShelterAddress = shelterAddress;
            Sterilization = sterilization;
            Weight = weight;
            Color = color;

            AddEvent(new PetUpdated(this));
        }
    }
}