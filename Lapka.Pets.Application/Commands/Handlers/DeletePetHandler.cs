using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.AspNetCore.Routing.Matching;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeletePetHandler : ICommandHandler<DeletePet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;


        public DeletePetHandler(IEventProcessor eventProcessor, IPetRepository petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
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
                //TODO: Microservice not responded or crashed, log here.
            }
            
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}