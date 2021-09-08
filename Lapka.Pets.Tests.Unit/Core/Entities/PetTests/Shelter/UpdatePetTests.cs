﻿using System;
using System.IO;
using System.Linq;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;
using Lapka.Pets.Core.ValueObjects;
using Shouldly;
using Xunit;
using File = Lapka.Pets.Core.ValueObjects.File;

namespace Lapka.Pets.Tests.Unit.Core.Entities.PetTests
{
    public class UpdatePetTests
    {
        [Fact]
        public void given_valid_pet_should_be_updated()
        {
            ShelterPet pet = Extensions.ArrangePet();
            
            pet.Update(pet.Name, pet.Race, pet.Species, pet.Sex, pet.BirthDay,
                pet.Sterilization, pet.Weight, pet.Color, pet.ShelterAddress, pet.Description);

            pet.ShouldNotBeNull();
            pet.Id.ShouldBe(pet.Id);
            pet.Name.ShouldBe(pet.Name);
            pet.Sex.ShouldBe(pet.Sex);
            pet.Race.ShouldBe(pet.Race);
            pet.Species.ShouldBe(pet.Species);
            pet.MainPhotoId.ShouldBe(pet.MainPhotoId);
            pet.BirthDay.ShouldBe(pet.BirthDay);
            pet.Color.ShouldBe(pet.Color);
            pet.Weight.ShouldBe(pet.Weight);
            pet.Sterilization.ShouldBe(pet.Sterilization);
            pet.ShelterAddress.ShouldBe(pet.ShelterAddress);
            pet.Description.ShouldBe(pet.Description);
            pet.Events.Count().ShouldBe(1);
            IDomainEvent @event = pet.Events.Single();
            @event.ShouldBeOfType<ShelterPetUpdated>();
        }
    }
}