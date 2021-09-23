using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;
using Shouldly;
using Xunit;

namespace Lapka.Pets.Tests.Unit.Core.Entities.PetTests.Shelters
{
    public class AddPetPhotoTests
    {
        [Fact]
        public void given_valid_pet_photos_should_be_added()
        {
            ShelterPet aggregatePet = Extensions.ArrangePet(photoIds: new HashSet<string>());
            ICollection<string> photoIds = new Collection<string>
            {
                "nowasciezka.jpg",
                "nowasciezka2.jpg",
            };
            
            aggregatePet.AddPhotos(photoIds);

            aggregatePet.ShouldNotBeNull();
            aggregatePet.Id.ShouldBe(aggregatePet.Id);
            aggregatePet.Name.ShouldBe(aggregatePet.Name);
            aggregatePet.Sex.ShouldBe(aggregatePet.Sex);
            aggregatePet.Race.ShouldBe(aggregatePet.Race);
            aggregatePet.Species.ShouldBe(aggregatePet.Species);
            aggregatePet.MainPhotoPath.ShouldBe(aggregatePet.MainPhotoPath);
            aggregatePet.PhotoPaths.Count().ShouldBe(2);
            aggregatePet.BirthDay.ShouldBe(aggregatePet.BirthDay);
            aggregatePet.Color.ShouldBe(aggregatePet.Color);
            aggregatePet.Weight.ShouldBe(aggregatePet.Weight);
            aggregatePet.Sterilization.ShouldBe(aggregatePet.Sterilization);
            aggregatePet.ShelterAddress.ShouldBe(aggregatePet.ShelterAddress);
            aggregatePet.Description.ShouldBe(aggregatePet.Description);
            foreach (string id in photoIds)
            {
                aggregatePet.PhotoPaths.ShouldContain(id);
            }
            aggregatePet.Events.Count().ShouldBe(1);
            IDomainEvent @event = aggregatePet.Events.Single();
            @event.ShouldBeOfType<ShelterPetUpdated>();
        }
    }
}