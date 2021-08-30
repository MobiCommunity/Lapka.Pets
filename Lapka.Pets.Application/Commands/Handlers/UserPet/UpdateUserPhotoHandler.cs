using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateUserPhotoHandler : ICommandHandler<UpdateUserPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<UserPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public UpdateUserPhotoHandler(IEventProcessor eventProcessor, IPetRepository<UserPet> petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(UpdateUserPetPhoto command)
        {
            UserPet pet = await _petRepository.GetByIdAsync(command.Id);
            if (pet is null)
            {
                throw new PetNotFoundException(command.Id);
            }

            try
            {
                await _grpcPhotoService.DeleteAsync(pet.MainPhotoId, BucketName.PetPhotos);
                await _grpcPhotoService.AddAsync(command.Photo.Id, command.Photo.Name, command.Photo.Content, BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            pet.UpdateMainPhoto(command.Photo.Id);

            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}