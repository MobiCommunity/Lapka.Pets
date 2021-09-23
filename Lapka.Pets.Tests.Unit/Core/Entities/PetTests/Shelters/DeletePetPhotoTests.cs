using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;
using Shouldly;
using Xunit;

namespace Lapka.Pets.Tests.Unit.Core.Entities.PetTests.Shelters
{
    public class DeletePetPhotoTests
    {
        [Fact]
        public void given_valid_pet_photo_path_should_be_deleted()
        {
            string photoId = "nowasciezka.jpg";
            ShelterPet pet = Extensions.ArrangePet(photoIds: new HashSet<string>
            {
                photoId
            });

            pet.RemovePhotos(pet.PhotoPaths);

            pet.ShouldNotBeNull();
            pet.Id.ShouldBe(pet.Id);
            pet.Name.ShouldBe(pet.Name);
            pet.Sex.ShouldBe(pet.Sex);
            pet.Race.ShouldBe(pet.Race);
            pet.Species.ShouldBe(pet.Species);
            pet.MainPhotoPath.ShouldBe(pet.MainPhotoPath);
            pet.PhotoPaths.Count().ShouldBe(0);
            pet.BirthDay.ShouldBe(pet.BirthDay);
            pet.Color.ShouldBe(pet.Color);
            pet.Weight.ShouldBe(pet.Weight);
            pet.Sterilization.ShouldBe(pet.Sterilization);
            pet.ShelterAddress.ShouldBe(pet.ShelterAddress);
            pet.Description.ShouldBe(pet.Description);
            pet.Events.Count().ShouldBe(1);
            IDomainEvent @event = pet.Events.Single();
            @event.ShouldBeOfType<ShelterPetPhotosDeleted>();
        }
    }
}