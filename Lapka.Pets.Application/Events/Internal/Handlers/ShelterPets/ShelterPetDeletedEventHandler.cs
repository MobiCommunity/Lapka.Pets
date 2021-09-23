using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Events.Abstract;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;

namespace Lapka.Pets.Application.Events.Internal.Handlers.ShelterPets
{
    public class ShelterPetDeletedEventHandler : IDomainEventHandler<ShelterPetDeleted>
    {
        private readonly IPetLikeElasticsearchUpdater _petLikeElasticsearchUpdater;
        private readonly IShelterPetElasticsearchUpdater _elasticsearchUpdater;
        private readonly IPetLikeRepository _petLikeRepository;

        public ShelterPetDeletedEventHandler(IPetLikeElasticsearchUpdater petLikeElasticsearchUpdater, 
            IShelterPetElasticsearchUpdater elasticsearchUpdater, IPetLikeRepository petLikeRepository)
        {
            _petLikeElasticsearchUpdater = petLikeElasticsearchUpdater;
            _elasticsearchUpdater = elasticsearchUpdater;
            _petLikeRepository = petLikeRepository;
        }

        public async Task HandleAsync(ShelterPetDeleted @event)
        {
            IEnumerable<UserLikedPets> petLikedLists =
                await _petLikeRepository.GetUsersLikedPetsContainingGivenPetAsync(@event.Pet.Id.Value);

            foreach (UserLikedPets likedPets in petLikedLists)
            {
                likedPets.RemoveLike(@event.Pet.Id.Value);
                await _petLikeRepository.UpdateLikesAsync(likedPets);
                await _petLikeElasticsearchUpdater.InsertAndUpdateDataAsync(likedPets);
            }

            await _elasticsearchUpdater.DeleteDataAsync(@event.Pet.Id.Value);
        }
    }
}