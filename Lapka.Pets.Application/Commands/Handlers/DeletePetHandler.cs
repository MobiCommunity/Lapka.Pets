using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeletePetHandler : ICommandHandler<DeletePet>
    {
        private readonly ILogger<DeletePetHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;


        public DeletePetHandler(ILogger<DeletePetHandler> logger, IEventProcessor eventProcessor, IPetRepository petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(DeletePet command)
        {
            Pet pet = await _petRepository.GetByIdAsync(command.Id);
            if (pet is null) throw new PetNotFoundException(command.Id);

            pet.Delete();

            await _petRepository.DeleteAsync(pet);
            
            try
            {
                await _grpcPhotoService.DeleteAsync(pet.MainPhotoPath);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}