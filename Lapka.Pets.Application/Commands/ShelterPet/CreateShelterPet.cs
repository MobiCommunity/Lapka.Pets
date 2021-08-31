using System;
using System.Collections.Generic;
using Lapka.Pets.Application.Commands.Abstract;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class CreateShelterPet : CreatePet
    {
        public Address ShelterAddress { get; }
        public string Description { get; }
        public bool Sterilization { get; }

        public CreateShelterPet(Guid id, string name, Sex sex, string race, Species species, PhotoFile photo,
            DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress,
            string description, IEnumerable<PhotoFile> photos) : base(id, name, sex, race, species, photo,
            birthDay, color, weight, photos)
        {
            Sterilization = sterilization;
            ShelterAddress = shelterAddress;
            Description = description;
        }
    }
}