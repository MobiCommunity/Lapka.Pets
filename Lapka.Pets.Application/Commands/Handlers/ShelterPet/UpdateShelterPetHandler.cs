using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateShelterPetHandler : ICommandHandler<UpdateShelterPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcIdentityService _grpcIdentityService;

        public UpdateShelterPetHandler(IEventProcessor eventProcessor, IShelterPetRepository repository,
            IGrpcIdentityService grpcIdentityService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _grpcIdentityService = grpcIdentityService;
        }

        public async Task HandleAsync(UpdateShelterPet command)
        {
            ShelterPet pet = await _repository.GetByIdAsync(command.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(command.Id);
            }

            bool isOwner = await _grpcIdentityService.IsUserOwnerOfShelter(pet.ShelterId, command.UserId);
            if (!isOwner)
            {
                throw new UserNotOwnerOfShelterException(command.UserId, pet.ShelterId);
            }

            pet.Update(command.Name, command.Race, command.Species, command.Sex, command.DateOfBirth,
                command.Sterilization, command.Weight, command.Color, command.ShelterAddress, command.Description);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}