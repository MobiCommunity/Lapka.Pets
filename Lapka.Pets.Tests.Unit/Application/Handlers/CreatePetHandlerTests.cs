using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.Handlers;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.ValueObjects;
using NSubstitute;
using Xunit;
using File = Lapka.Pets.Core.ValueObjects.File;

namespace Lapka.Pets.Tests.Unit.Application.Handlers
{
    public class CreatePetHandlerTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly CreatePetHandler _handler;
        private readonly IPetRepository _petRepository;

        public CreatePetHandlerTests()
        {
            _petRepository = Substitute.For<IPetRepository>();
            _grpcPhotoService = Substitute.For<IGrpcPhotoService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new CreatePetHandler(_eventProcessor, _petRepository, _grpcPhotoService);
        }

        private Task Act(CreatePet command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_resource_id_for_valid_customer_reserve_resource_should_succeed()
        {
            Pet pet = ArrangePet();
            File file = ArrangeFile();

            CreatePet command = new CreatePet(pet.Id.Value, pet.Name, pet.Sex, pet.Race, pet.Species, file,
                pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress, pet.Description);

            Pet petCreated = Pet.Create(pet.Id.Value, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoPath,
                pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress, pet.Description);
            File fileCreated = new File(file.Name, file.Content, file.ContentType);

            await _grpcPhotoService.AddAsync(fileCreated.Name, fileCreated.Content);
            await _petRepository.AddAsync(petCreated);

            await Act(command);

            await _petRepository.Received().AddAsync(Arg.Is(petCreated));
            await _grpcPhotoService.Received().AddAsync(Arg.Is(fileCreated.Name), Arg.Is(fileCreated.Content));
            await _eventProcessor.Received().ProcessAsync(Arg.Any<IEnumerable<IDomainEvent>>());
        }

        private Pet ArrangePet(AggregateId id = null, string name = null, Sex? sex = null, string race = null,
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

            Pet pet = new Pet(validId.Value, validName, validSex, validRace, validSpecies, validPhotoPath,
                validBirthDate, validColor, validWeight, validSterilization, validShelterAddress, validDescription);

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

        private File ArrangeFile(string name = null, Stream stream = null, string contentType = null)
        {
            string validName = name ?? $"{Guid.NewGuid()}.jpg";
            Stream validStream = stream ?? new MemoryStream();
            string validContentType = contentType ?? "ocet/jpg";

            File file = new File(validName, validStream, validContentType);

            return file;
        }
    }
}