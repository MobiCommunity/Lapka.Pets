using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Events.External.Handlers
{
    public class ShelterAddedHandler : IEventHandler<ShelterAdded>
    {
        private readonly IShelterRepository _shelterRepository;
        private readonly IGrpcIdentityService _grpcIdentityService;

        public ShelterAddedHandler(IShelterRepository shelterRepository, IGrpcIdentityService grpcIdentityService)
        {
            _shelterRepository = shelterRepository;
            _grpcIdentityService = grpcIdentityService;
        }

        public async Task HandleAsync(ShelterAdded @event)
        {
            ShelterBasicInfo shelterBasicInfo = await _grpcIdentityService.GetShelterBasicInfo(@event.ShelterId);
            Shelter shelter = Shelter.Create(@event.ShelterId, shelterBasicInfo.Name, shelterBasicInfo.Address,
                shelterBasicInfo.Location);

            await _shelterRepository.CreateAsync(shelter);
        }
    }
}