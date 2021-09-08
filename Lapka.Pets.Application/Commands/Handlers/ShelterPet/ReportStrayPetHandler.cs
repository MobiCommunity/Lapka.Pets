using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class ReportStrayPetHandler : ICommandHandler<ReportStrayPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _shelterPetRepository;
        private readonly IGrpcIdentityService _grpcIdentityService;
        private readonly IGrpcPhotoService _photoService;


        public ReportStrayPetHandler(IEventProcessor eventProcessor, IShelterPetRepository shelterPetRepository,
            IGrpcIdentityService grpcIdentityService, IGrpcPhotoService photoService)
        {
            _eventProcessor = eventProcessor;
            _shelterPetRepository = shelterPetRepository;
            _grpcIdentityService = grpcIdentityService;
            _photoService = photoService;
        }

        public async Task HandleAsync(ReportStrayPet command)
        {
            Guid shelter = await _grpcIdentityService.ClosetShelter(command.Location.Longitude.Value,
                    command.Location.Latitude.Value);

            //TODO: Send message to closest shelter
            throw new System.NotImplementedException();
        }
    }
}