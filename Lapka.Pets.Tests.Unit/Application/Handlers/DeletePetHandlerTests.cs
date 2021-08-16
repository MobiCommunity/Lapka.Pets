using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.Handlers;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Lapka.Pets.Tests.Unit.Application.Handlers
{
    public class DeletePetHandlerTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly DeletePetHandler _handler;
        private readonly IPetRepository _petRepository;
        private readonly ILogger<DeletePetHandler> _logger;

        public DeletePetHandlerTests()
        {
            _petRepository = Substitute.For<IPetRepository>();
            _grpcPhotoService = Substitute.For<IGrpcPhotoService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _logger = Substitute.For<ILogger<DeletePetHandler>>();
            _handler = new DeletePetHandler(_logger, _eventProcessor, _petRepository, _grpcPhotoService);
        }

        private Task Act(DeletePet command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_should_delete()
        {
            Pet pet = ArrangePet();
            DeletePet command = new DeletePet(pet.Id.Value);

            _petRepository.GetByIdAsync(command.Id).Returns(pet);

            await Act(command);

            await _petRepository.Received().DeleteAsync(pet);
            await _grpcPhotoService.Received().DeleteAsync(pet.MainPhotoPath);
            await _eventProcessor.Received().ProcessAsync(pet.Events);
        }
        
        [Fact]
        public async Task given_valid_pet_with_multiple_photos_should_delete()
        {
            Pet pet = ArrangePet();
            DeletePet command = new DeletePet(pet.Id.Value);

            _petRepository.GetByIdAsync(command.Id).Returns(pet);

            await Act(command);

            await _petRepository.Received().DeleteAsync(pet);
            await _grpcPhotoService.Received().DeleteAsync(pet.MainPhotoPath);
            foreach (string photo in pet.PhotoPaths)
            {
                await _grpcPhotoService.Received().DeleteAsync(photo);
            }
            await _eventProcessor.Received().ProcessAsync(pet.Events);
        }
        
        private Pet ArrangePet(AggregateId id = null, string name = null, Sex? sex = null, string race = null,
            Species? species = null, string photoPath = null, DateTime? birthDay = null, string color = null,
            double? weight = null, bool? sterilization = null, Address shelterAddress = null, string description = null,
            List<string> photoPaths = null)
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
            List<string> validPhotoPaths = photoPaths;
            if (validPhotoPaths is null)
            {
                validPhotoPaths = new List<string>();
                for (int i = 0; i < 3; i++)
                {
                    validPhotoPaths.Add($"{Guid.NewGuid()}.jpg");
                }
            }

            Pet pet = new Pet(validId.Value, validName, validSex, validRace, validSpecies, validPhotoPath,
                validBirthDate, validColor, validWeight, validSterilization, validShelterAddress, validDescription,
                validPhotoPaths);

            return pet;
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
    }
}