using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdatePetPhotoHandler : ICommandHandler<UpdatePetPhoto>
    {
        private readonly ILogger<UpdatePetPhotoHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public UpdatePetPhotoHandler(ILogger<UpdatePetPhotoHandler> logger, IEventProcessor eventProcessor,
            IPetRepository petRepository, IGrpcPhotoService grpcPhotoService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }
        
        public async Task HandleAsync(UpdatePetPhoto command)
        {
            string mainPhotoPath = $"{command.PhotoId:N}.{command.Photo.GetFileExtension()}"; 

            Pet pet = await _petRepository.GetByIdAsync(command.Id);
            if (pet is null)
            {
                throw new PetNotFoundException(command.Id);
            }
            
            try
            {
                await _grpcPhotoService.DeleteAsync(pet.MainPhotoPath);
                await _grpcPhotoService.AddAsync(mainPhotoPath, command.Photo.Content);
            }
            catch(Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            pet.UpdateMainPhoto(mainPhotoPath);
            
            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}