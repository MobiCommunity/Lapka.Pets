using System;
using System.Collections.Generic;
using Lapka.Pets.Application.Commands.Abstract;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class CreateShelterPet : CreatePet
    {
        public string UserId { get; }
        public Address ShelterAddress { get; }
        public string Description { get; }

        public CreateShelterPet(Guid id, string userId, string name, Sex sex, string race, Species species, PhotoFile photo,
            DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress,
            string description, IEnumerable<PhotoFile> photos) : base(id, name, sex, race, species, photo,
            birthDay, color, weight, sterilization, photos)
        {
            UserId = userId;
            ShelterAddress = shelterAddress;
            Description = description;
        }
    }
}