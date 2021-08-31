using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class AddLostPetPhotoHandler : ICommandHandler<AddLostPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<LostPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public AddLostPetPhotoHandler(IEventProcessor eventProcessor, IPetRepository<LostPet> petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }
        public async Task HandleAsync(AddLostPetPhoto command)
        {
            LostPet pet = await PetHelpers.GetPetFromRepositoryAsync(_petRepository, command.PetId);

            foreach (PhotoFile photo in command.Photos)
            {
                await PetHelpers.AddPetPhotoAsync(_grpcPhotoService, photo);
            }
            pet.AddPhotos(command.Photos.IdsAsGuidList());
            
            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}