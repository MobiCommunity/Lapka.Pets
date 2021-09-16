using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;
using Lapka.Pets.Core.Events.Concrete.Pets.Users;

namespace Lapka.Pets.Application.Events.Internal.Handlers.UserPets
{
    public class UserPetDeletedEventHandler : IDomainEventHandler<UserPetDeleted>
    {
        private readonly IUserPetElasticsearchUpdater _elasticsearchUpdater;

        public UserPetDeletedEventHandler(IUserPetElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        
        public async Task HandleAsync(UserPetDeleted @event)
        {
            await _elasticsearchUpdater.DeleteDataAsync(@event.Pet.Id.Value);
        }
    }
}