using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;

namespace Lapka.Pets.Application.Events.Internal.Handlers.ShelterPets
{
    public class ShelterPetUpdatedEventHandler : IDomainEventHandler<ShelterPetUpdated>
    {
        private readonly IShelterPetElasticsearchUpdater _elasticsearchUpdater;

        public ShelterPetUpdatedEventHandler(IShelterPetElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        
        public async Task HandleAsync(ShelterPetUpdated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Pet);
        }
    }
}