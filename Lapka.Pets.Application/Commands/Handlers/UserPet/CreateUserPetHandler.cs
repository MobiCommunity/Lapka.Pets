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
    public class CreateUserPetHandler : ICommandHandler<CreateUserPet>
    {
        private readonly ILogger<CreateUserPetHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<UserPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IGrpcPetService _grpcPetService;

        public CreateUserPetHandler(ILogger<CreateUserPetHandler> logger, IEventProcessor eventProcessor,
            IPetRepository<UserPet> petRepository, IGrpcPhotoService grpcPhotoService, IGrpcPetService grpcPetService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
            _grpcPetService = grpcPetService;
        }

        public async Task HandleAsync(CreateUserPet command)
        {
            UserPet pet = UserPet.Create(command.Id, command.UserId, command.Name, command.Sex, command.Race,
                command.Species, command.MainPhoto.Id, command.BirthDay, command.Color, command.Weight,
                command.Sterilization, command.Photos.IdsAsGuidList());

            try
            {
                await _grpcPetService.AddPetAsync(command.UserId, command.Id);
            }
            catch (Exception e)
            {
                throw new CannotRequestPetsMicroserviceException(e);
            }

            try
            {
                await _grpcPhotoService.AddAsync(command.MainPhoto.Id, command.MainPhoto.Name,
                    command.MainPhoto.Content, BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                pet.UpdateMainPhoto(command.MainPhoto.Id);

                await _petRepository.UpdateAsync(pet);
            }

            await _petRepository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}