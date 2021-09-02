using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Infrastructure.PetServices.User
{
    public class UserPetService : IUserPetService
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserPetRepository _repository;
        private readonly IUserPetPhotoService _petPhotoService;

        public UserPetService(IEventProcessor eventProcessor, IUserPetRepository repository, IUserPetPhotoService petPhotoService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _petPhotoService = petPhotoService;
        }
        
        public async Task<UserPet> GetAsync(Guid petId)
        {
            UserPet pet = await _repository.GetByIdAsync(petId);
            if (pet == null)
            {
                throw new PetNotFoundException(petId);
            }

            return pet;
        }

        public Task<IEnumerable<UserPet>> GetAllUserPets(Guid userId)
        {
            return _repository.GetUserPets(userId);
        }

        public void ValidIfUserIsOwnerOfPet(UserPet pet, Guid userId)
        {
            if (pet.UserId != userId)
            {
                throw new PetDoesNotBelongToUserException(userId.ToString(), pet.Id.Value.ToString());
            }
        }
        public async Task AddAsync<THandler>(ILogger<THandler> logger, PhotoFile mainPhoto, List<PhotoFile> photoFiles, UserPet pet)
        {
            await _petPhotoService.AddPetPhotosAsync(logger, mainPhoto, photoFiles, pet);

            await _repository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        public async Task DeleteAsync<THandler>(ILogger<THandler> logger, UserPet pet)
        {
            await _petPhotoService.DeletePetPhotosAsync(logger, pet.MainPhotoId, pet.PhotoIds);
            pet.Delete();
            
            await _repository.DeleteAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
        
        public async Task UpdateAsync(UserPet pet)
        {
            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}