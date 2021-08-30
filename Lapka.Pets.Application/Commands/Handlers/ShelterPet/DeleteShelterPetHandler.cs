using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteShelterPetHandler : ICommandHandler<DeleteShelterPet>
    {
        private readonly ILogger<DeleteShelterPetHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IPetRepository<ShelterPet> _petRepository;


        public DeleteShelterPetHandler(ILogger<DeleteShelterPetHandler> logger, IEventProcessor eventProcessor,
            IPetRepository<ShelterPet> petRepository, IGrpcPhotoService grpcPhotoService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(DeleteShelterPet command)
        {
            ShelterPet pet = await _petRepository.GetByIdAsync(command.Id);
            if (pet is null)
            {
                throw new PetNotFoundException(command.Id);
            }

            pet.Delete();
            
            try
            {
                await _grpcPhotoService.DeleteAsync(pet.MainPhotoId, BucketName.PetPhotos);
                foreach (Guid photoId in pet.PhotoIds)
                {
                    await _grpcPhotoService.DeleteAsync(photoId, BucketName.PetPhotos);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            
            await _petRepository.DeleteAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}