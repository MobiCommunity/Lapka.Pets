using System.Threading.Tasks;
using Convey.CQRS.Events;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Events.External.Handlers
{
    public class ShelterOwnerAssignedHandler : IEventHandler<ShelterOwnerAssigned>
    {
        private readonly IShelterRepository _shelterRepository;

        public ShelterOwnerAssignedHandler(IShelterRepository shelterRepository)
        {
            _shelterRepository = shelterRepository;
        }

        public async Task HandleAsync(ShelterOwnerAssigned @event)
        {
            Shelter shelter = await _shelterRepository.GetAsync(@event.ShelterId);
            if (shelter is null)
            {
                throw new ShelterDoesNotExistsException(@event.ShelterId);
            }
            
            shelter.AddOwner(@event.UserId);
            await _shelterRepository.UpdateAsync(shelter);
        }
    }
}