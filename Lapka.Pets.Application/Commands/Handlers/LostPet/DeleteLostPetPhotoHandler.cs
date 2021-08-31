using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteLostPetPhotoHandler : ICommandHandler<DeleteLostPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<LostPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public DeleteLostPetPhotoHandler(IEventProcessor eventProcessor, IPetRepository<LostPet> petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }
        public async Task HandleAsync(DeleteLostPetPhoto command)
        {
            LostPet pet = await PetHelpers.GetPetFromRepositoryAsync(_petRepository, command.PetId);
            PetHelpers.CheckIfPhotoExist(command.PhotoId, pet);

            await PetHelpers.DeletePetPhotoAsync(_grpcPhotoService, command.PhotoId);
            pet.RemovePhoto(command.PhotoId);
            
            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}