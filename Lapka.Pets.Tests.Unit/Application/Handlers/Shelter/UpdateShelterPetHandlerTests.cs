using System;
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
    public class UpdateShelterPetHandlerTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly UpdateShelterPetHandler _handler;
        private readonly IPetRepository<ShelterPet> _petRepository;

        public UpdateShelterPetHandlerTests()
        {
            _petRepository = Substitute.For<IPetRepository<ShelterPet>>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new UpdateShelterPetHandler(_eventProcessor, _petRepository);
        }

        private Task Act(UpdateShelterPet command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_pet_should_update()
        {
            ShelterPet arrangePet = Extensions.ArrangePet();

            ShelterPet pet = ShelterPet.Create(arrangePet.Id.Value, arrangePet.Name, arrangePet.Sex, arrangePet.Race,
                arrangePet.Species, arrangePet.MainPhotoId, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                arrangePet.Sterilization, arrangePet.ShelterAddress, arrangePet.Description, arrangePet.PhotoIds);

            UpdateShelterPet command = new UpdateShelterPet(arrangePet.Id.Value, arrangePet.Name, arrangePet.Race,
                arrangePet.Species,
                arrangePet.Sex, arrangePet.BirthDay, arrangePet.Description, arrangePet.ShelterAddress,
                arrangePet.Sterilization, arrangePet.Weight, arrangePet.Color);

            _petRepository.GetByIdAsync(command.Id).Returns(pet);

            await Act(command);

            await _petRepository.Received().UpdateAsync(pet);
            await _eventProcessor.Received().ProcessAsync(pet.Events);
        }
    }
}