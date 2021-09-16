using System;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.Handlers;
using Lapka.Pets.Application.Commands.Handlers.ShelterPets;
using Lapka.Pets.Application.Commands.ShelterPets;
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
        private readonly UpdateShelterPetHandler _handler;
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IGrpcIdentityService _grpcIdentity;
        public UpdateShelterPetHandlerTests()
        {          
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterPetRepository>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _grpcIdentity = Substitute.For<IGrpcIdentityService>();

            _handler = new UpdateShelterPetHandler(_eventProcessor, _repository, _grpcIdentity);
        }

        private Task Act(UpdateShelterPet command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_pet_should_update()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet arrangePet = Extensions.ArrangePet(userId: userId);

            ShelterPet pet = ShelterPet.Create(arrangePet.Id.Value, arrangePet.UserId, arrangePet.Name, arrangePet.Sex, arrangePet.Race,
                arrangePet.Species, arrangePet.MainPhotoId, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                arrangePet.Sterilization, arrangePet.ShelterId, arrangePet.ShelterAddress, arrangePet.Description, arrangePet.PhotoIds);

            UpdateShelterPet command = new UpdateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name, arrangePet.Race,
                arrangePet.Species,
                arrangePet.Sex, arrangePet.BirthDay, arrangePet.Description, arrangePet.ShelterAddress,
                arrangePet.Sterilization, arrangePet.Weight, arrangePet.Color);

            _repository.GetByIdAsync(command.Id).Returns(pet);
            _grpcIdentity.IsUserOwnerOfShelter(pet.ShelterId, command.UserId).Returns(true);

            await Act(command);

            await _repository.Received().UpdateAsync(pet);
        }
    }
}