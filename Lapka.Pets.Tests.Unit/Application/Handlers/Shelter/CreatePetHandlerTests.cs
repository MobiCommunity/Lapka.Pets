﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.Handlers;
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
        private readonly IShelterPetService _petService;
        private readonly ILogger<CreateShelterPetHandler> _logger;

        public CreatePetHandlerTests()
        {
            _logger = Substitute.For<ILogger<CreateShelterPetHandler>>();
            _petService = Substitute.For<IShelterPetService>();
            _handler = new CreateShelterPetHandler(_logger, _petService);
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

            ShelterPet pet = Extensions.ArrangePet(photoId: mainPhoto.Id, photoIds: photos.IdsAsGuidList());

            CreateShelterPet command = new CreateShelterPet(pet.Id.Value, pet.Name, pet.Sex, pet.Race,
                pet.Species, mainPhoto, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress,
                pet.Description, photos);

            await Act(command);

            await _petService.Received()
                .AddAsync(_logger, mainPhoto, null,Arg.Is<ShelterPet>(p => p.Id.Value == pet.Id.Value &&
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
        public async Task given_valid_pet_without_photos_should_create()
        {
            PhotoFile photo = Extensions.ArrangePhotoFile();
            ShelterPet pet = Extensions.ArrangePet(photoId: photo.Id, photoIds: new List<Guid>());

            CreateShelterPet command = new CreateShelterPet(pet.Id.Value, pet.Name, pet.Sex, pet.Race,
                pet.Species, photo, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress,
                pet.Description, null);

            await Act(command);

            await _petService.Received()
                .AddAsync(Arg.Is(_logger), Arg.Is(photo), null,Arg.Is<ShelterPet>(p => p.Id.Value == pet.Id.Value &&
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

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, arrangePet.Name, arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, mainPhoto, arrangePet.BirthDay, arrangePet.Color,
                arrangePet.Weight, arrangePet.Sterilization, arrangePet.ShelterAddress, arrangePet.Description, null);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPetNameException>();
        }

        [Fact]
        public async Task given_invalid_pet_race_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(race: "");
            PhotoFile file = Extensions.ArrangePhotoFile();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, arrangePet.Name, arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                arrangePet.Sterilization, arrangePet.ShelterAddress, arrangePet.Description, null);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRaceValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_birth_date_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(birthDay: DateTime.Now.Add(TimeSpan.FromMinutes(1)));
            PhotoFile file = Extensions.ArrangePhotoFile();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, arrangePet.Name, arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                arrangePet.Sterilization, arrangePet.ShelterAddress, arrangePet.Description, null);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidBirthDayValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_color_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(color: "");
            PhotoFile file = Extensions.ArrangePhotoFile();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, arrangePet.Name,
                arrangePet.Sex, arrangePet.Race, arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color,
                arrangePet.Weight, arrangePet.Sterilization, arrangePet.ShelterAddress, arrangePet.Description, null);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidColorValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_weight_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(weight: 0);
            PhotoFile file = Extensions.ArrangePhotoFile();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, arrangePet.Name, arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                arrangePet.Sterilization, arrangePet.ShelterAddress, arrangePet.Description, null);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<WeightBelowMinimumValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_description_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(description: "");
            PhotoFile file = Extensions.ArrangePhotoFile();

            Exception exception = await Record.ExceptionAsync(async () => await Act(
                new CreateShelterPet(arrangePet.Id.Value, arrangePet.Name, arrangePet.Sex, arrangePet.Race,
                    arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                    arrangePet.Sterilization, arrangePet.ShelterAddress, arrangePet.Description, null)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidDescriptionValueException>();
        }
    }
}