using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;

namespace Lapka.Pets.Application.Events.Internal.Handlers.ShelterPets
{
    public class ShelterPetCreatedEventHandler : IDomainEventHandler<ShelterPetCreated>
    {
        private readonly IShelterPetElasticsearchUpdater _elasticsearchUpdater;

        public ShelterPetCreatedEventHandler(IShelterPetElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(ShelterPetCreated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Pet);
        }
    }
}