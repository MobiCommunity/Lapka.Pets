using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete.Pets.Like;

namespace Lapka.Pets.Application.Events.Internal.Handlers.Likes
{
    public class DislikePetEventHandler : IDomainEventHandler<RemovedPetLike>
    {
        private readonly IPetLikeElasticsearchUpdater _elasticsearchUpdater;

        public DislikePetEventHandler(IPetLikeElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }

        public async Task HandleAsync(RemovedPetLike @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.LikedPets);
        }
    }
}