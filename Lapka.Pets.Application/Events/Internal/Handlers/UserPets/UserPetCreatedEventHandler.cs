using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;
using Lapka.Pets.Core.Events.Concrete.Pets.Users;

namespace Lapka.Pets.Application.Events.Internal.Handlers.UserPets
{
    public class UserPetCreatedEventHandler : IDomainEventHandler<UserPetCreated>
    {
        private readonly IUserPetElasticsearchUpdater _elasticsearchUpdater;

        public UserPetCreatedEventHandler(IUserPetElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(UserPetCreated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Pet);
        }
    }
}