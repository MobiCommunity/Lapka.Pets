using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateLostPetPhotoHandler : ICommandHandler<UpdateLostPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<LostPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public UpdateLostPetPhotoHandler(IEventProcessor eventProcessor, IPetRepository<LostPet> petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(UpdateLostPetPhoto command)
        {
            LostPet pet = await PetHelpers.GetPetFromRepositoryAsync(_petRepository, command.Id);
            
            await PetHelpers.DeletePetPhotoAsync(_grpcPhotoService, pet.MainPhotoId);
            await PetHelpers.AddPetPhotoAsync(_grpcPhotoService, command.Photo);
            pet.UpdateMainPhoto(command.Photo.Id);

            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}