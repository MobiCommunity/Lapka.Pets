using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete.Pets.Like;

namespace Lapka.Pets.Application.Events.Internal.Handlers.Likes
{
    public class LikePetEventHandler : IDomainEventHandler<AddedPetLike>
    {
        private readonly IPetLikeElasticsearchUpdater _elasticsearchUpdater;

        public LikePetEventHandler(IPetLikeElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }

        public async Task HandleAsync(AddedPetLike @event)
        {
            await _elasticsearchUpdater.DeleteDataAsync(@event.LikedPets);
        }
    }
}