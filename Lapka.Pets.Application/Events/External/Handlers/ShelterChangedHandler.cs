using System.Threading.Tasks;
using Convey.CQRS.Events;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Events.External.Handlers
{
    public class ShelterChangedHandler : IEventHandler<ShelterChanged>
    {
        private readonly IShelterRepository _shelterRepository;
        private readonly IGrpcIdentityService _grpcIdentityService;

        public ShelterChangedHandler(IShelterRepository shelterRepository, IGrpcIdentityService grpcIdentityService)
        {
            _shelterRepository = shelterRepository;
            _grpcIdentityService = grpcIdentityService;
        }

        public async Task HandleAsync(ShelterChanged @event)
        {
            Shelter shelter = await GetShelterAsync(@event);

            ShelterBasicInfo shelterBasicInfo = await _grpcIdentityService.GetShelterBasicInfo(@event.ShelterId);

            shelter.UpdateName(shelterBasicInfo.Name);
            await _shelterRepository.UpdateAsync(shelter);
        }

        private async Task<Shelter> GetShelterAsync(ShelterChanged @event)
        {
            Shelter shelter = await _shelterRepository.GetAsync(@event.ShelterId);
            if (shelter is null)
            {
                throw new ShelterDoesNotExistsException(@event.ShelterId);
            }

            return shelter;
        }
    }
}