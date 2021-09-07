using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Services;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetLikedPetsHandler : IQueryHandler<GetLikedPets, IEnumerable<PetBasicShelterDto>>
    {
        private readonly IMongoRepository<ShelterPetDocument, Guid> _shelterRepository;
        private readonly IMongoRepository<LikePetDocument, Guid> _likeRepository;

        public GetLikedPetsHandler(IMongoRepository<ShelterPetDocument, Guid> shelterRepository,
            IMongoRepository<LikePetDocument, Guid> likeRepository)
        {
            _shelterRepository = shelterRepository;
            _likeRepository = likeRepository;
        }

        public async Task<IEnumerable<PetBasicShelterDto>> HandleAsync(GetLikedPets query)
        {
            List<PetBasicShelterDto> pets = new List<PetBasicShelterDto>();
            IMongoQueryable<ShelterPetDocument> queryable = _shelterRepository.Collection.AsQueryable();
            LikePetDocument likedPets = await _likeRepository.GetAsync(x => x.Id == query.UserId);

            foreach (Guid petId in likedPets.LikedPets)
            {
                ShelterPetDocument pet = await queryable.FirstOrDefaultAsync(x => x.Id == petId);
                pets.Add(pet.AsBasicDto(query.Latitude, query.Longitude));
            }

            return pets;
        }
    }
}