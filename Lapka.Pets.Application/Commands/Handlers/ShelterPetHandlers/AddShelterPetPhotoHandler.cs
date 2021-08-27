﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class AddShelterPetPhotoHandler : ICommandHandler<AddShelterPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<ShelterPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public AddShelterPetPhotoHandler(IEventProcessor eventProcessor, IPetRepository<ShelterPet> petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }
        public async Task HandleAsync(AddShelterPetPhoto command)
        {
            List<string> photoPaths = new List<string>();
            
            ShelterPet pet = await _petRepository.GetByIdAsync(command.PetId);
            if (pet is null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            foreach (File photo in command.Photos)
            {
                string photoPath = $"{Guid.NewGuid():N}.{photo.GetFileExtension()}";
                photoPaths.Add(photoPath);
                
                try
                {
                    await _grpcPhotoService.AddAsync(photoPath, photo.Content, BucketName.PetPhotos);
                }
                catch(Exception ex)
                {
                    throw new CannotRequestFilesMicroserviceException(ex);
                }
            }

            pet.AddPhotos(photoPaths);
            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}