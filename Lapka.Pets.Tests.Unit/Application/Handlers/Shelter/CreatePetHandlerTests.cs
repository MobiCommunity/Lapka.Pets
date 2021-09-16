using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.Handlers;
using Lapka.Pets.Application.Commands.Handlers.ShelterPets;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;
using File = Lapka.Pets.Core.ValueObjects.File;
using ILogger = Castle.Core.Logging.ILogger;

namespace Lapka.Pets.Tests.Unit.Application.Handlers
{
    public class CreatePetHandlerTests
    {
        private readonly CreateShelterPetHandler _handler;
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IGrpcIdentityService _grpcIdentity;
        private readonly ILogger<CreateShelterPetHandler> _logger;

        public CreatePetHandlerTests()
        {
            _logger = Substitute.For<ILogger<CreateShelterPetHandler>>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterPetRepository>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _grpcIdentity = Substitute.For<IGrpcIdentityService>();

            _handler = new CreateShelterPetHandler(_logger, _eventProcessor, _repository, _photoService, _grpcIdentity);
        }

        private Task Act(CreateShelterPet command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_pet_should_create()
        {
            List<PhotoFile> photos = new List<PhotoFile>
            {
                Extensions.ArrangePhotoFile(),
                Extensions.ArrangePhotoFile()
            };
            PhotoFile mainPhoto = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();

            ShelterPet pet =
                Extensions.ArrangePet(photoId: mainPhoto.Id, photoIds: photos.IdsAsGuidList(), userId: userId);

            CreateShelterPet command = new CreateShelterPet(pet.Id.Value, pet.UserId, pet.Name, pet.Sex, pet.Race,
                pet.Species, mainPhoto, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterId,
                pet.ShelterAddress, pet.Description, photos);

            _grpcIdentity.IsUserOwnerOfShelter(command.ShelterId, userId).Returns(true);

            await Act(command);

            await _repository.Received()
                .AddAsync(Arg.Is<ShelterPet>(p => p.Id.Value == pet.Id.Value && p.UserId == pet.UserId &&
                                                  p.Name == pet.Name && p.Sex == pet.Sex &&
                                                  p.Race == pet.Race && p.Species == pet.Species &&
                                                  p.BirthDay == pet.BirthDay &&
                                                  p.Color == pet.Color && p.Weight == pet.Weight &&
                                                  p.Sterilization == pet.Sterilization &&
                                                  p.ShelterAddress.City ==
                                                  pet.ShelterAddress.City &&
                                                  p.ShelterAddress.Name ==
                                                  pet.ShelterAddress.Name &&
                                                  p.ShelterAddress.Street ==
                                                  pet.ShelterAddress.Street &&
                                                  p.ShelterAddress.GeoLocation.Latitude ==
                                                  pet.ShelterAddress.GeoLocation.Latitude &&
                                                  p.ShelterAddress.GeoLocation.Longitude ==
                                                  pet.ShelterAddress.GeoLocation.Longitude &&
                                                  p.Description == pet.Description &&
                                                  p.MainPhotoId == pet.MainPhotoId &&
                                                  p.PhotoIds.SequenceEqual(pet.PhotoIds)));
            await _photoService.Received().AddAsync(Arg.Is(mainPhoto.Id), Arg.Is(mainPhoto.Name),
                Arg.Is(mainPhoto.Content), Arg.Is(BucketName.PetPhotos));
            foreach (PhotoFile photo in photos)
            {
                await _photoService.Received().AddAsync(Arg.Is(photo.Id), Arg.Is(photo.Name),
                    Arg.Is(photo.Content), Arg.Is(BucketName.PetPhotos));
            }
        }

        [Fact]
        public async Task given_valid_pet_without_photos_should_create()
        {
            Guid userId = Guid.NewGuid();
            PhotoFile photo = Extensions.ArrangePhotoFile();
            ShelterPet pet = Extensions.ArrangePet(photoId: photo.Id, photoIds: new List<Guid>(), userId: userId);

            CreateShelterPet command = new CreateShelterPet(pet.Id.Value, pet.UserId, pet.Name, pet.Sex, pet.Race,
                pet.Species, photo, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterId,
                pet.ShelterAddress,
                pet.Description, null);

            _grpcIdentity.IsUserOwnerOfShelter(command.ShelterId, userId).Returns(true);

            await Act(command);

            await _repository.Received()
                .AddAsync(Arg.Is<ShelterPet>(p => p.Id.Value == pet.Id.Value && p.UserId == pet.UserId &&
                                                  p.Name == pet.Name && p.Sex == pet.Sex &&
                                                  p.Race == pet.Race && p.Species == pet.Species &&
                                                  p.BirthDay == pet.BirthDay &&
                                                  p.Color == pet.Color && p.Weight == pet.Weight &&
                                                  p.Sterilization == pet.Sterilization &&
                                                  p.ShelterAddress.City ==
                                                  pet.ShelterAddress.City &&
                                                  p.ShelterAddress.Name ==
                                                  pet.ShelterAddress.Name &&
                                                  p.ShelterAddress.Street ==
                                                  pet.ShelterAddress.Street &&
                                                  p.ShelterAddress.GeoLocation.Latitude ==
                                                  pet.ShelterAddress.GeoLocation.Latitude &&
                                                  p.ShelterAddress.GeoLocation.Longitude ==
                                                  pet.ShelterAddress.GeoLocation.Longitude &&
                                                  p.Description == pet.Description &&
                                                  p.MainPhotoId == pet.MainPhotoId &&
                                                  p.PhotoIds.SequenceEqual(pet.PhotoIds)));
        }

        [Fact]
        public async Task given_invalid_pet_name_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(name: "");
            PhotoFile mainPhoto = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name,
                arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, mainPhoto, arrangePet.BirthDay, arrangePet.Color,
                arrangePet.Weight, arrangePet.Sterilization, arrangePet.ShelterId, arrangePet.ShelterAddress,
                arrangePet.Description, null);
            
            _grpcIdentity.IsUserOwnerOfShelter(arrangePet.ShelterId, userId).Returns(true);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPetNameException>();
        }

        [Fact]
        public async Task given_invalid_pet_race_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(race: "");
            PhotoFile file = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name,
                arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                arrangePet.Sterilization, arrangePet.ShelterId, arrangePet.ShelterAddress, arrangePet.Description,
                null);
            
            _grpcIdentity.IsUserOwnerOfShelter(arrangePet.ShelterId, userId).Returns(true);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRaceValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_birth_date_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(birthDay: DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)));
            PhotoFile file = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name,
                arrangePet.Sex, arrangePet.Race, arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color,
                arrangePet.Weight, arrangePet.Sterilization, arrangePet.ShelterId, arrangePet.ShelterAddress,
                arrangePet.Description, null);
            
            _grpcIdentity.IsUserOwnerOfShelter(arrangePet.ShelterId, command.UserId).Returns(true);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidBirthDayValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_color_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(color: "");
            PhotoFile file = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name,
                arrangePet.Sex, arrangePet.Race, arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color,
                arrangePet.Weight, arrangePet.Sterilization, arrangePet.ShelterId, arrangePet.ShelterAddress,
                arrangePet.Description, null);
            
            _grpcIdentity.IsUserOwnerOfShelter(arrangePet.ShelterId, userId).Returns(true);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidColorValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_weight_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(weight: 0);
            PhotoFile file = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name,
                arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                arrangePet.Sterilization, arrangePet.ShelterId, arrangePet.ShelterAddress, arrangePet.Description,
                null);
            
            _grpcIdentity.IsUserOwnerOfShelter(arrangePet.ShelterId, command.UserId).Returns(true);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<WeightBelowMinimumValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_description_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(description: "");
            PhotoFile file = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();
            
            _grpcIdentity.IsUserOwnerOfShelter(arrangePet.ShelterId, userId).Returns(true);

            Exception exception = await Record.ExceptionAsync(async () => await Act(
                new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name, arrangePet.Sex, arrangePet.Race,
                    arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                    arrangePet.Sterilization, arrangePet.ShelterId, arrangePet.ShelterAddress, arrangePet.Description,
                    null)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidDescriptionValueException>();
        }
    }
}