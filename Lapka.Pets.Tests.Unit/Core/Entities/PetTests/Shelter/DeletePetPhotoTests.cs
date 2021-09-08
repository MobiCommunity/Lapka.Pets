using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;
using Lapka.Pets.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Pets.Tests.Unit.Core.Entities.PetTests
{
    public class DeletePetPhotoTests
    {
        [Fact]
        public void given_valid_pet_photo_path_should_be_deleted()
        {
            Guid photoId = Guid.NewGuid();
            ShelterPet pet = Extensions.ArrangePet(photoIds: new List<Guid>
            {
                photoId
            });
            Guid photoPath = pet.PhotoIds.First();

            pet.RemovePhoto(photoPath);

            pet.ShouldNotBeNull();
            pet.Id.ShouldBe(pet.Id);
            pet.Name.ShouldBe(pet.Name);
            pet.Sex.ShouldBe(pet.Sex);
            pet.Race.ShouldBe(pet.Race);
            pet.Species.ShouldBe(pet.Species);
            pet.MainPhotoId.ShouldBe(pet.MainPhotoId);
            pet.PhotoIds.Count().ShouldBe(0);
            pet.BirthDay.ShouldBe(pet.BirthDay);
            pet.Color.ShouldBe(pet.Color);
            pet.Weight.ShouldBe(pet.Weight);
            pet.Sterilization.ShouldBe(pet.Sterilization);
            pet.ShelterAddress.ShouldBe(pet.ShelterAddress);
            pet.Description.ShouldBe(pet.Description);
            pet.Events.Count().ShouldBe(1);
            IDomainEvent @event = pet.Events.Single();
            @event.ShouldBeOfType<ShelterPetPhotoDeleted>();
        }
    }
}