using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteLostPetHandler : ICommandHandler<DeleteLostPet>
    {
        private readonly ILogger<DeleteLostPetHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IPetRepository<LostPet> _petRepository;


        public DeleteLostPetHandler(ILogger<DeleteLostPetHandler> logger, IEventProcessor eventProcessor,
            IPetRepository<LostPet> petRepository, IGrpcPhotoService grpcPhotoService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(DeleteLostPet command)
        {
            LostPet pet = await PetHelpers.GetPetFromRepositoryAsync(_petRepository, command.PetId);
            
            pet.Delete();
            await PetHelpers.DeletePetPhotosAsync(_logger, _grpcPhotoService, pet.MainPhotoId, pet.PhotoIds);
            
            await _petRepository.DeleteAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}