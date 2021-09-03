using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Infrastructure.PetServices.Shelter
{
    public class ShelterPetService : IShelterPetService
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IShelterPetPhotoService _petPhotoService;

        public ShelterPetService(IEventProcessor eventProcessor, IShelterPetRepository repository, IShelterPetPhotoService petPhotoService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _petPhotoService = petPhotoService;
        }
        
        public async Task<ShelterPet> GetAsync(Guid petId)
        {
            ShelterPet pet = await _repository.GetByIdAsync(petId);
            if (pet == null)
            {
                throw new PetNotFoundException(petId);
            }

            return pet;
        }
        
        public async Task AddAsync<THandler>(ILogger<THandler> logger, PhotoFile mainPhoto, List<PhotoFile> photoFiles, ShelterPet pet)
        {
            await _petPhotoService.AddPetPhotosAsync(logger, mainPhoto, photoFiles, pet);

            await _repository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        public async Task DeleteAsync<THandler>(ILogger<THandler> logger, ShelterPet pet)
        {
            await _petPhotoService.DeletePetPhotosAsync(logger, pet.MainPhotoId, pet.PhotoIds);
            pet.Delete();
            
            await _repository.DeleteAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
        public async Task UpdateAsync(ShelterPet pet)
        {
            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}