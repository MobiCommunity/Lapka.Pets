﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeletePetPhotoHandler : ICommandHandler<DeletePetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public DeletePetPhotoHandler(IEventProcessor eventProcessor, IPetRepository petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }
        public async Task HandleAsync(DeletePetPhoto command)
        {
            Pet pet = await _petRepository.GetByIdAsync(command.PetId);
            if (pet is null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            string photoPath = pet.PhotoPaths.FirstOrDefault(x =>
                x.Equals(command.Path, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(photoPath))
            {
                throw new PhotoNotFoundException(photoPath);
            }
            
            try
            {
                await _grpcPhotoService.DeleteAsync(command.Path);
            }
            catch(Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            pet.RemovePhoto(command.Path);
            
            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}