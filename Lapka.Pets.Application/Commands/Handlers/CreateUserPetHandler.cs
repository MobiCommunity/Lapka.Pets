using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
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

        public CreateUserPetHandler(ILogger<CreateUserPetHandler> logger,IEventProcessor eventProcessor, IPetRepository<UserPet> petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(CreateUserPet command)
        {
            string mainPhotoPath = $"{command.PhotoId:N}.{command.Photo.GetFileExtension()}"; 
            
            UserPet pet = UserPet.Create(command.Id, command.Name, command.Sex, command.Race, command.Species,
                mainPhotoPath, command.BirthDay, command.Color, command.Weight, command.Sterilization);

            
            await _petRepository.AddAsync(pet);
            
            try
            {
                await _grpcPhotoService.AddAsync(mainPhotoPath, command.Photo.Content, BucketName.PetPhotos);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                
                pet.Update(pet.Name, pet.Race, pet.Species, pet.MainPhotoPath, pet.Sex, pet.BirthDay, 
                    pet.Sterilization, pet.Weight, pet.Color);

                await _petRepository.UpdateAsync(pet);
            }
            
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}