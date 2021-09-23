using System.Threading.Tasks;
using Convey.CQRS.Events;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Events.External.Handlers
{
    public class ShelterRemovedHandler : IEventHandler<ShelterRemoved>
    {
        private readonly IShelterRepository _shelterRepository;

        public ShelterRemovedHandler(IShelterRepository shelterRepository)
        {
            _shelterRepository = shelterRepository;
        }

        public async Task HandleAsync(ShelterRemoved @event)
        {
            Shelter shelter = await _shelterRepository.GetAsync(@event.ShelterId);
            if (shelter is null)
            {
                throw new ShelterDoesNotExistsException(@event.ShelterId);
            }
            
            await _shelterRepository.DeleteAsync(shelter);
        }
    }
}