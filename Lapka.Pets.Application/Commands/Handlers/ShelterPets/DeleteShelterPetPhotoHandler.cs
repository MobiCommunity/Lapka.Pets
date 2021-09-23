using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers.ShelterPets
{
    public class DeleteShelterPetPhotoHandler : ICommandHandler<DeleteShelterPetPhoto>
    {
        private readonly IShelterPetRepository _repository;
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterRepository _shelterRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;


        public DeleteShelterPetPhotoHandler(IShelterPetRepository repository, IEventProcessor eventProcessor,
            IShelterRepository shelterRepository, IMessageBroker messageBroker,
            IDomainToIntegrationEventMapper eventMapper)
        {
            _repository = repository;
            _eventProcessor = eventProcessor;
            _shelterRepository = shelterRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(DeleteShelterPetPhoto command)
        {
            ShelterPet pet = await GetShelterPetAsync(command);
            await ValidIfUserOwnShelter(command, pet);
            ValidIfPhotosExists(command, pet);
            
            pet.RemovePhotos(command.PhotoPaths);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(pet.Events);
            await _messageBroker.PublishAsync(events);
        }

        private static void ValidIfPhotosExists(DeleteShelterPetPhoto command, ShelterPet pet)
        {
            foreach (string path in command.PhotoPaths)
            {
                if (!pet.PhotoPaths.Contains(path))
                {
                    throw new PhotoNotFoundException(path);
                }
            }
        }

        private async Task<ShelterPet> GetShelterPetAsync(DeleteShelterPetPhoto command)
        {
            ShelterPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet is null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            return pet;
        }

        private async Task ValidIfUserOwnShelter(DeleteShelterPetPhoto command, ShelterPet pet)
        {
            Shelter shelter = await _shelterRepository.GetAsync(pet.ShelterId);
            if (shelter.Owners.Any(x => x != command.UserId))
            {
                throw new UserNotOwnerOfShelterException(command.UserId, pet.ShelterId);
            }
        }
    }
}