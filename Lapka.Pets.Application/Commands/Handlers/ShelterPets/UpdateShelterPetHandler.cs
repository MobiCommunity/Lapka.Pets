using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers.ShelterPets
{
    public class UpdateShelterPetHandler : ICommandHandler<UpdateShelterPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IShelterRepository _shelterRepository;

        public UpdateShelterPetHandler(IEventProcessor eventProcessor, IShelterPetRepository repository,
            IShelterRepository shelterRepository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _shelterRepository = shelterRepository;
        }

        public async Task HandleAsync(UpdateShelterPet command)
        {
            ShelterPet pet = await GetShelterPetAsync(command);
            await ValidIfUserOwnShelter(command, pet);
            
            pet.Update(command.Name, command.Race, command.Species, command.Sex, command.DateOfBirth,
                command.Sterilization, command.Weight, command.Color, command.ShelterAddress, command.Description);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private async Task<ShelterPet> GetShelterPetAsync(UpdateShelterPet command)
        {
            ShelterPet pet = await _repository.GetByIdAsync(command.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(command.Id);
            }

            return pet;
        }

        private async Task ValidIfUserOwnShelter(UpdateShelterPet command, ShelterPet pet)
        {
            Shelter shelter = await _shelterRepository.GetAsync(pet.ShelterId);
            if (shelter.Owners.Any(x => x != command.UserId))
            {
                throw new UserNotOwnerOfShelterException(command.UserId, pet.ShelterId);
            }
        }
    }
}