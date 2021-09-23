using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands.Handlers.ShelterPets;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Lapka.Pets.Tests.Unit.Application.Handlers.Shelters
{
    public class CreatePetHandlerTests
    {
        private readonly CreateShelterPetHandler _handler;
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IShelterRepository _shelterRepository;
        private readonly ILogger<CreateShelterPetHandler> _logger;

        public CreatePetHandlerTests()
        {
            _logger = Substitute.For<ILogger<CreateShelterPetHandler>>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterPetRepository>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _shelterRepository = Substitute.For<IShelterRepository>();

            _handler = new CreateShelterPetHandler(_logger, _eventProcessor, _repository, _photoService,
                _shelterRepository);
        }

        private Task Act(CreateShelterPet command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_pet_should_create()
        {
            List<string> photoPaths = new List<string>();

            List<File> photos = new List<File>
            {
                Extensions.ArrangePhotoFile(),
                Extensions.ArrangePhotoFile()
            };
            File mainPhoto = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();
            Shelter shelter = Extensions.ArrangeShelter(owners: new HashSet<Guid>
            {
                userId
            });
            ShelterPet pet = Extensions.ArrangePet(userId: userId);

            CreateShelterPet command = new CreateShelterPet(pet.Id.Value, pet.UserId, pet.Name, pet.Sex, pet.Race,
                pet.Species, mainPhoto, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.Description,
                pet.ShelterId, photos);

            _shelterRepository.GetAsync(command.ShelterId).Returns(shelter);
            _photoService.AddAsync(mainPhoto.Name, userId, true, mainPhoto.Content, BucketName.PetPhotos)
                .Returns(mainPhoto.Name);

            foreach (File photo in photos)
            {
                _photoService.AddAsync(Arg.Is(photo.Name), Arg.Is(userId),
                    Arg.Is(true), Arg.Is(photo.Content), Arg.Is(BucketName.PetPhotos)).Returns(photo.Name);
                photoPaths.Add(photo.Name);
            }

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
                                                  p.ShelterGeoLocation.Latitude.Value ==
                                                  pet.ShelterGeoLocation.Latitude.Value &&
                                                  p.ShelterGeoLocation.Longitude.Value ==
                                                  pet.ShelterGeoLocation.Longitude.Value &&
                                                  p.Description == pet.Description &&
                                                  p.MainPhotoPath == mainPhoto.Name &&
                                                  p.PhotoPaths.SequenceEqual(photoPaths)));
        }

        [Fact]
        public async Task given_valid_pet_without_photos_should_create()
        {
            Guid userId = Guid.NewGuid();
            File photo = Extensions.ArrangePhotoFile();
            ShelterPet pet = Extensions.ArrangePet(userId: userId);
            Shelter shelter = Extensions.ArrangeShelter(owners: new HashSet<Guid>
            {
                userId
            });

            CreateShelterPet command = new CreateShelterPet(pet.Id.Value, pet.UserId, pet.Name, pet.Sex, pet.Race,
                pet.Species, photo, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.Description,
                pet.ShelterId);
            
            _photoService.AddAsync(photo.Name, userId, true, photo.Content, BucketName.PetPhotos)
                .Returns(photo.Name);
            _shelterRepository.GetAsync(command.ShelterId).Returns(shelter);

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
                                                  p.ShelterGeoLocation.Latitude.Value ==
                                                  pet.ShelterGeoLocation.Latitude.Value &&
                                                  p.ShelterGeoLocation.Longitude.Value ==
                                                  pet.ShelterGeoLocation.Longitude.Value &&
                                                  p.Description == pet.Description &&
                                                  p.MainPhotoPath == photo.Name &&
                                                  p.PhotoPaths.SequenceEqual(Enumerable.Empty<string>())));
        }

        [Fact]
        public async Task given_invalid_pet_name_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(name: "");
            File mainPhoto = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();
            Shelter shelter = Extensions.ArrangeShelter(owners: new HashSet<Guid>
            {
                userId
            });

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name,
                arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, mainPhoto, arrangePet.BirthDay, arrangePet.Color,
                arrangePet.Weight, arrangePet.Sterilization, arrangePet.Description, arrangePet.ShelterId);

            _shelterRepository.GetAsync(command.ShelterId).Returns(shelter);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPetNameException>();
        }

        [Fact]
        public async Task given_invalid_pet_race_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(race: "");
            File file = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();
            Shelter shelter = Extensions.ArrangeShelter(owners: new HashSet<Guid>
            {
                userId
            });

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name,
                arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                arrangePet.Sterilization, arrangePet.Description, arrangePet.ShelterId);

            _shelterRepository.GetAsync(command.ShelterId).Returns(shelter);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRaceValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_birth_date_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(birthDay: DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)));
            File file = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();
            Shelter shelter = Extensions.ArrangeShelter(owners: new HashSet<Guid>
            {
                userId
            });

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name,
                arrangePet.Sex, arrangePet.Race, arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color,
                arrangePet.Weight, arrangePet.Sterilization, arrangePet.Description, arrangePet.ShelterId);

            _shelterRepository.GetAsync(command.ShelterId).Returns(shelter);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidBirthDayValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_color_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(color: "");
            File file = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();
            Shelter shelter = Extensions.ArrangeShelter(owners: new HashSet<Guid>
            {
                userId
            });

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name,
                arrangePet.Sex, arrangePet.Race, arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color,
                arrangePet.Weight, arrangePet.Sterilization, arrangePet.Description, arrangePet.ShelterId);

            _shelterRepository.GetAsync(command.ShelterId).Returns(shelter);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidColorValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_weight_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(weight: 0);
            File file = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();
            Shelter shelter = Extensions.ArrangeShelter(owners: new HashSet<Guid>
            {
                userId
            });

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name,
                arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                arrangePet.Sterilization, arrangePet.Description, arrangePet.ShelterId);

            _shelterRepository.GetAsync(command.ShelterId).Returns(shelter);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<WeightBelowMinimumValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_description_should_throw_an_exception()
        {
            ShelterPet arrangePet = Extensions.ArrangePet(description: "");
            File file = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();
            Shelter shelter = Extensions.ArrangeShelter(owners: new HashSet<Guid>
            {
                userId
            });

            _shelterRepository.GetAsync(arrangePet.ShelterId).Returns(shelter);

            Exception exception = await Record.ExceptionAsync(async () => await Act(
                new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name, arrangePet.Sex, arrangePet.Race,
                    arrangePet.Species, file, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                    arrangePet.Sterilization, arrangePet.Description, arrangePet.ShelterId)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidDescriptionValueException>();
        }
    }
}