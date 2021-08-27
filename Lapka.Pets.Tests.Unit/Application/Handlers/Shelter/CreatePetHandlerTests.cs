using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.Handlers;
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

namespace Lapka.Pets.Tests.Unit.Application.Handlers
{
    public class CreatePetHandlerTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly CreateShelterPetHandler _handler;
        private readonly ILogger<CreateShelterPetHandler> _logger;
        private readonly IPetRepository<ShelterPet> _petRepository;

        public CreatePetHandlerTests()
        {
            _petRepository = Substitute.For<IPetRepository<ShelterPet>>();
            _grpcPhotoService = Substitute.For<IGrpcPhotoService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _logger = Substitute.For<ILogger<CreateShelterPetHandler>>();
            _handler = new CreateShelterPetHandler(_eventProcessor, _petRepository, _grpcPhotoService, _logger);
        }

        private Task Act(CreateShelterPet command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_pet_should_create()
        {
            ShelterPet pet = ArrangePet();
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();
            string userId = Guid.NewGuid().ToString();

            CreateShelterPet command = new CreateShelterPet(pet.Id.Value, userId, pet.Name, pet.Sex, pet.Race,
                pet.Species, file, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress, pet.Description, photoId);

            await Act(command);

            string fileNameExpectedValue = $"{photoId:N}.jpg";

            await _petRepository.Received()
                .AddAsync(Arg.Is<ShelterPet>(p => p.Id.Value == pet.Id.Value &&
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
                                                  p.MainPhotoPath == fileNameExpectedValue));

            await _grpcPhotoService.Received().AddAsync(Arg.Is(fileNameExpectedValue), Arg.Is(file.Content),
                Arg.Is(BucketName.PetPhotos));
            await _eventProcessor.Received().ProcessAsync(Arg.Is<IEnumerable<IDomainEvent>>(e
                => e.FirstOrDefault().GetType() == typeof(PetCreated<ShelterPet>)));
        }

        [Fact]
        public async Task given_invalid_pet_name_should_throw_an_exception()
        {
            ShelterPet arrangePet = ArrangePet(name: "");
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();
            string userId = Guid.NewGuid().ToString();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name, arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, file,
                arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight, arrangePet.Sterilization,
                arrangePet.ShelterAddress, arrangePet.Description, photoId);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPetNameException>();
        }

        [Fact]
        public async Task given_invalid_pet_race_should_throw_an_exception()
        {
            ShelterPet arrangePet = ArrangePet(race: "");
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();
            string userId = Guid.NewGuid().ToString();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name, arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, file,
                arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight, arrangePet.Sterilization,
                arrangePet.ShelterAddress, arrangePet.Description, photoId);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRaceValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_birth_date_should_throw_an_exception()
        {
            ShelterPet arrangePet = ArrangePet(birthDay: DateTime.Now.Add(TimeSpan.FromMinutes(1)));
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();
            string userId = Guid.NewGuid().ToString();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name, arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, file,
                arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight, arrangePet.Sterilization,
                arrangePet.ShelterAddress, arrangePet.Description, photoId);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidBirthDayValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_color_should_throw_an_exception()
        {
            ShelterPet arrangePet = ArrangePet(color: "");
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();
            string userId = Guid.NewGuid().ToString();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name, arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, file,
                arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight, arrangePet.Sterilization,
                arrangePet.ShelterAddress, arrangePet.Description, photoId);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidColorValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_weight_should_throw_an_exception()
        {
            ShelterPet arrangePet = ArrangePet(weight: 0);
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();
            string userId = Guid.NewGuid().ToString();

            CreateShelterPet command = new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name, arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, file,
                arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight, arrangePet.Sterilization,
                arrangePet.ShelterAddress, arrangePet.Description, photoId);

            Exception exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<WeightBelowMinimumValueException>();
        }

        [Fact]
        public async Task given_invalid_pet_description_should_throw_an_exception()
        {
            ShelterPet arrangePet = ArrangePet(description: "");
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();
            string userId = Guid.NewGuid().ToString();

            Exception exception = await Record.ExceptionAsync(async () => await Act(
                new CreateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name, arrangePet.Sex, arrangePet.Race,
                    arrangePet.Species, file, arrangePet.BirthDay,
                    arrangePet.Color, arrangePet.Weight, arrangePet.Sterilization, arrangePet.ShelterAddress,
                    arrangePet.Description, photoId)));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidDescriptionValueException>();
        }

        private ShelterPet ArrangePet(AggregateId id = null, string name = null, Sex? sex = null, string race = null,
            Species? species = null, string photoPath = null, DateTime? birthDay = null, string color = null,
            double? weight = null, bool? sterilization = null, Address shelterAddress = null, string description = null)
        {
            AggregateId validId = id ?? new AggregateId();
            string validName = name ?? "Miniok";
            Sex validSex = sex ?? Sex.Male;
            string validRace = race ?? "mops";
            Species validSpecies = species ?? Species.Dog;
            string validPhotoPath = photoPath ?? $"{Guid.NewGuid()}.jpg";
            DateTime validBirthDate = birthDay ?? DateTime.Now.Subtract(TimeSpan.FromDays(180));
            string validColor = color ?? "red";
            double validWeight = weight ?? 152;
            bool validSterilization = sterilization ?? true;
            string validDescription = description ?? "Dlugi opis nie do przeczytania.";
            Address validShelterAddress = shelterAddress ?? ArrangeShelterAddress();

            ShelterPet aggregatePet = new ShelterPet(validId.Value, validName, validSex, validRace, validSpecies,
                validPhotoPath,
                validBirthDate, validColor, validWeight, validSterilization, validShelterAddress, validDescription);

            return aggregatePet;
        }

        private Address ArrangeShelterAddress(string name = null, string city = null, string street = null,
            Location location = null)
        {
            string shelterAddressName = name ?? "Mokotovo";
            string shelterAddressCity = city ?? "Rzeszow";
            string shelterAddressStreet = street ?? "Wojskowa";
            Location validShelterLocation = location ?? ArrangeShelterAddressLocation();

            Address address = new Address(shelterAddressName, shelterAddressCity, shelterAddressStreet,
                validShelterLocation);

            return address;
        }

        private Location ArrangeShelterAddressLocation(string latitude = null, string longitude = null)
        {
            string shelterAddressLocationLatitude = latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";

            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }

        private File ArrangeFile(string name = null, Stream stream = null, string contentType = null)
        {
            string validName = name ?? $"{Guid.NewGuid()}.jpg";
            Stream validStream = stream ?? new MemoryStream();
            string validContentType = contentType ?? "image/jpg";

            File file = new File(validName, validStream, validContentType);

            return file;
        }
    }
}