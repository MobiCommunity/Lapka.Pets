using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class CreatePetHandler : ICommandHandler<CreatePet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public CreatePetHandler(IEventProcessor eventProcessor, IPetRepository petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }
        
        public async Task HandleAsync(CreatePet command)
        {
            string mainPhotoPath = $"{Guid.NewGuid():N}.{command.Photo.GetFileExtension()}"; 
            
            Pet pet = Pet.Create(command.Id, command.Name, command.Sex, command.Race, command.Species, mainPhotoPath,
                command.BirthDay, command.Color, command.Weight, command.Sterilization, command.ShelterAddress,
                command.Description);
            
            await _petRepository.AddAsync(pet);
            
            try
            {
                await _grpcPhotoService.AddAsync(mainPhotoPath, command.Photo.Content);
            }
            catch(Exception ex)
            {
                //TODO: Microservice not responded or crashed, log here.
                
                pet.Update(pet.Name, pet.Race, pet.Species, "", pet.Sex, pet.BirthDay, pet.Description,
                    pet.ShelterAddress, pet.Sterilization, pet.Weight, pet.Color);

                await _petRepository.UpdateAsync(pet);
            }
            
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}