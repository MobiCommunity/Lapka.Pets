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

namespace Lapka.Pets.Tests.Unit.Core.Entities.PetTests.Shelter
{
    public class AddPetPhotoTests
    {
        [Fact]
        public void given_valid_pet_photos_should_be_added()
        {
            ShelterPet aggregatePet = Extensions.ArrangePet(photoIds: new List<Guid>());
            List<Guid> photoIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            
            aggregatePet.AddPhotos(photoIds);

            aggregatePet.ShouldNotBeNull();
            aggregatePet.Id.ShouldBe(aggregatePet.Id);
            aggregatePet.Name.ShouldBe(aggregatePet.Name);
            aggregatePet.Sex.ShouldBe(aggregatePet.Sex);
            aggregatePet.Race.ShouldBe(aggregatePet.Race);
            aggregatePet.Species.ShouldBe(aggregatePet.Species);
            aggregatePet.MainPhotoId.ShouldBe(aggregatePet.MainPhotoId);
            aggregatePet.PhotoIds.Count().ShouldBe(2);
            aggregatePet.BirthDay.ShouldBe(aggregatePet.BirthDay);
            aggregatePet.Color.ShouldBe(aggregatePet.Color);
            aggregatePet.Weight.ShouldBe(aggregatePet.Weight);
            aggregatePet.Sterilization.ShouldBe(aggregatePet.Sterilization);
            aggregatePet.ShelterAddress.ShouldBe(aggregatePet.ShelterAddress);
            aggregatePet.Description.ShouldBe(aggregatePet.Description);
            foreach (Guid id in photoIds)
            {
                aggregatePet.PhotoIds.ShouldContain(id);
            }
            aggregatePet.Events.Count().ShouldBe(1);
            IDomainEvent @event = aggregatePet.Events.Single();
            @event.ShouldBeOfType<ShelterPetUpdated>();
        }
    }
}