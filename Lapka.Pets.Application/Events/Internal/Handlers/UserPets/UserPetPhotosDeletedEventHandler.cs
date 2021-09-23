using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete.Pets.Users;

namespace Lapka.Pets.Application.Events.Internal.Handlers.UserPets
{
    public class UserPetPhotosDeletedEventHandler : IDomainEventHandler<UserPetPhotosDeleted>
    {
        private readonly IUserPetElasticsearchUpdater _elasticsearchUpdater;

        public UserPetPhotosDeletedEventHandler(IUserPetElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        
        public async Task HandleAsync(UserPetPhotosDeleted @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Pet);
        }
    }
}