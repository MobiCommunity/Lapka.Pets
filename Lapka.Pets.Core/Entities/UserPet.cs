using System;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class UserPet : Pet
    {
        public UserPet(Guid id, string name, Sex sex, string race, Species species, string photoPath, DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress, string description) : base(id, name, sex, race, species, photoPath, birthDay, color, weight, sterilization, shelterAddress, description)
        {
        }
    }
}