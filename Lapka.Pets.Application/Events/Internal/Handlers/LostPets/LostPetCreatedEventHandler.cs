using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete.Pets.Losts;

namespace Lapka.Pets.Application.Events.Internal.Handlers.LostPets
{
    public class LostPetCreatedEventHandler : IDomainEventHandler<LostPetCreated>
    {
        private readonly ILostPetElasticsearchUpdater _elasticsearchUpdater;

        public LostPetCreatedEventHandler(ILostPetElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(LostPetCreated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Pet);
        }
    }
}