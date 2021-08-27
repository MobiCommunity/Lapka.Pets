using System;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class ShelterPet : AggregatePet
    {
        public Address ShelterAddress { get; private set; }
        public string Description { get; private set; }

        public ShelterPet(Guid id, string name, Sex sex, string race, Species species, string photoPath,
            DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress,
            string description) : base(id, name, sex, race, species, photoPath, birthDay, color, weight, sterilization)
        {
            ShelterAddress = shelterAddress;
            Description = description;
        }

        public static ShelterPet Create(Guid id, string name, Sex sex, string race, Species species, string photoPath,
            DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress,
            string description)
        {
            Validate(name, race, birthDay, color, weight, description);
            ShelterPet pet = new ShelterPet(id, name, sex, race, species, photoPath, birthDay, color, weight,
                sterilization, shelterAddress, description);

            pet.AddEvent(new PetCreated<ShelterPet>(pet));
            return pet;
        }

        public void Update(string name, string race, Species species, string photoPath, Sex sex, DateTime birthDay,
            bool sterilization, double weight, string color, Address shelterAddress, string description)
        {
            Update(name, race, species, photoPath, sex, birthDay, sterilization, weight, color);
            Validate(name, race, birthDay, color, weight, description);

            ShelterAddress = shelterAddress;
            Description = description;

            AddEvent(new PetUpdated<ShelterPet>(this));
        }

        public override void Delete()
        {
            AddEvent(new PetDeleted<ShelterPet>(this));
        }

        private static void Validate(string name, string race, DateTime birthDay, string color, double weight,
            string description)
        {
            Validate(name, race, birthDay, color, weight);

            ValidateDescription(description);
        }
        
        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                throw new InvalidDescriptionValueException(description);
        }
    }
}