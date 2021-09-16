using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete;

namespace Lapka.Pets.Application.Events.Internal.Handlers.LostPets
{
    public class LostPetUpdatedEventHandler : IDomainEventHandler<LostPetUpdated>
    {
        private readonly ILostPetElasticsearchUpdater _elasticsearchUpdater;

        public LostPetUpdatedEventHandler(ILostPetElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        
        public async Task HandleAsync(LostPetUpdated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Pet);
        }
    }
}