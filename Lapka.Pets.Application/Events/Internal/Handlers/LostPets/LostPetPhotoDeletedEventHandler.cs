using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Events.Concrete.Pets.Losts;

namespace Lapka.Pets.Application.Events.Internal.Handlers.LostPets
{
    public class LostPetPhotosUpdatedEventHandler : IDomainEventHandler<LostPetPhotosDeleted>
    {
        private readonly ILostPetElasticsearchUpdater _elasticsearchUpdater;

        public LostPetPhotosUpdatedEventHandler(ILostPetElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        
        public async Task HandleAsync(LostPetPhotosDeleted @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Pet);
        }
    }
}