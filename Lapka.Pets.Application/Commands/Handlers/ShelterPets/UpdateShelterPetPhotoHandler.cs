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
    public class UpdateShelterPetPhotoHandler : ICommandHandler<UpdateShelterPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IShelterRepository _shelterRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;


        public UpdateShelterPetPhotoHandler(IEventProcessor eventProcessor, IShelterPetRepository repository,
            IGrpcPhotoService photoService, IShelterRepository shelterRepository, IMessageBroker messageBroker,
            IDomainToIntegrationEventMapper eventMapper)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
            _shelterRepository = shelterRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(UpdateShelterPetPhoto command)
        {
            ShelterPet pet = await GetShelterPetAsync(command);
            await ValidIfUserOwnShelter(command, pet);
            string oldPhotoPath = pet.MainPhotoPath;

            string path = await AddPhotoToMinioAsync(command);

            pet.UpdateMainPhoto(path, oldPhotoPath);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(pet.Events);
            await _messageBroker.PublishAsync(events);
        }

        private async Task<ShelterPet> GetShelterPetAsync(UpdateShelterPetPhoto command)
        {
            ShelterPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            return pet;
        }

        private async Task ValidIfUserOwnShelter(UpdateShelterPetPhoto command, ShelterPet pet)
        {
            Shelter shelter = await _shelterRepository.GetAsync(pet.ShelterId);
            if (shelter.Owners.Any(x => x != command.UserId))
            {
                throw new UserNotOwnerOfShelterException(command.UserId, pet.ShelterId);
            }
        }
        
        private async Task<string> AddPhotoToMinioAsync(UpdateShelterPetPhoto command)
        {
            string path;
            try
            {
                path = await _photoService.AddAsync(command.Photo.Name, command.UserId, true, command.Photo.Content,
                    BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            return path;
        }
    }
}