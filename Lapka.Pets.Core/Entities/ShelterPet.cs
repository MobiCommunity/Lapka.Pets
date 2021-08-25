using System;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class ShelterPet : Pet
    {
        public Address ShelterAddress { get; private set; }

        public string Description { get; private set; }

        public ShelterPet(Guid id, string name, Sex sex, string race, Species species, string photoPath, DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress, string description) : base(id, name, sex, race, species, photoPath, birthDay, color, weight, sterilization, shelterAddress, description)
        {
            ShelterAddress = shelterAddress;
            Description = description;
        }

        public override T Create<T>(Guid id, string name, Sex sex, string race, Species species, string photoPath, DateTime birthDay,
            string color, double weight, bool sterilization, Address shelterAddress, string description)
        {
            Validate(name, race, birthDay, color, weight, description);
        
            ShelterPet pet = new ShelterPet(id, name, sex, race, species, photoPath, birthDay, color, weight, sterilization,
                shelterAddress, description);
        
            pet.AddEvent(new PetCreated(pet));
            return pet;        
        }
    }
}