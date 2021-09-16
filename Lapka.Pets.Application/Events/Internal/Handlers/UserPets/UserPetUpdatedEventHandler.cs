using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete.Pets.Users;

namespace Lapka.Pets.Application.Events.Internal.Handlers.UserPets
{
    public class UserPetUpdatedEventHandler : IDomainEventHandler<UserPetUpdated>
    {
        private readonly IUserPetElasticsearchUpdater _elasticsearchUpdater;

        public UserPetUpdatedEventHandler(IUserPetElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        
        public async Task HandleAsync(UserPetUpdated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Pet);
        }
    }
}