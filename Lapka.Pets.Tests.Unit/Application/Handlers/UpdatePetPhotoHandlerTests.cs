using System;
using System.IO;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.Handlers;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using File = Lapka.Pets.Core.ValueObjects.File;

namespace Lapka.Pets.Tests.Unit.Application.Handlers
{
    public class UpdatePetPhotoHandlerTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly UpdatePetPhotoHandler _handler;
        private readonly IPetRepository _petRepository;
        private readonly ILogger<UpdatePetPhotoHandler> _logger;

        public UpdatePetPhotoHandlerTests()
        {
            _petRepository = Substitute.For<IPetRepository>();
            _grpcPhotoService = Substitute.For<IGrpcPhotoService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _logger = Substitute.For<ILogger<UpdatePetPhotoHandler>>();
            _handler = new UpdatePetPhotoHandler(_logger, _eventProcessor, _petRepository, _grpcPhotoService);
        }

        private Task Act(UpdatePetPhoto command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_photo_should_update()
        {
            Pet pet = ArrangePet();
            File file = ArrangeFile();
            Guid photoId = Guid.NewGuid();
            string photoIdBeforeUpdated = pet.MainPhotoPath;
            UpdatePetPhoto command = new UpdatePetPhoto(pet.Id.Value, file, photoId);
            
            _petRepository.GetByIdAsync(command.Id).Returns(pet);

            await Act(command);
            
            string fileNameExpectedValue = $"{photoId:N}.jpg";
            await _petRepository.Received().UpdateAsync(pet);
            await _grpcPhotoService.Received().DeleteAsync(photoIdBeforeUpdated);
            await _grpcPhotoService.Received().AddAsync(Arg.Is(fileNameExpectedValue), Arg.Is(file.Content));
            await _eventProcessor.Received().ProcessAsync(pet.Events);
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
            string validContentType = contentType ?? "image/jpg";

            File file = new File(validName, validStream, validContentType);

            return file;
        }
    }
}