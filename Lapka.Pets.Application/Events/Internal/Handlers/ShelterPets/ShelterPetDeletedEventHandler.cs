using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;

namespace Lapka.Pets.Application.Events.Internal.Handlers.ShelterPets
{
    public class ShelterPetDeletedEventHandler : IDomainEventHandler<ShelterPetDeleted>
    {
        private readonly IShelterPetElasticsearchUpdater _elasticsearchUpdater;

        public ShelterPetDeletedEventHandler(IShelterPetElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        
        public async Task HandleAsync(ShelterPetDeleted @event)
        {
            await _elasticsearchUpdater.DeleteDataAsync(@event.Pet.Id.Value);
        }
    }
}