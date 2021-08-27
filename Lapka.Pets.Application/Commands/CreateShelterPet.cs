using System;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class CreateShelterPet : CreatePet
    {
        public string? UserId { get; }
        public Address ShelterAddress { get; }
        public string Description { get; }

        public CreateShelterPet(Guid id, string? userId, string name, Sex sex, string race, Species species, File photo,
            DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress,
            string description) : base(id, name, sex, race, species, photo, birthDay, color, weight, sterilization)
        {
            UserId = userId;
            ShelterAddress = shelterAddress;
            Description = description;
        }
    }
}