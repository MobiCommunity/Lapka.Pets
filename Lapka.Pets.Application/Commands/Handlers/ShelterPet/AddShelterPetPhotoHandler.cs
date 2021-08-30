using System;
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
            ShelterPet pet = await _petRepository.GetByIdAsync(command.PetId);
            if (pet is null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            foreach (PhotoFile photo in command.Photos)
            {
                try
                {
                    await _grpcPhotoService.AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
                }
                catch(Exception ex)
                {
                    throw new CannotRequestFilesMicroserviceException(ex);
                }
            }

            pet.AddPhotos(command.Photos.IdsAsGuidList());
            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}