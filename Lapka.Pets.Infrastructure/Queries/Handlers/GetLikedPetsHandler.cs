using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.PetDtos;
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
        private readonly IPetLikesService _petLikesService;

        public GetLikedPetsHandler(IMongoRepository<ShelterPetDocument, Guid> shelterRepository, IPetLikesService petLikesService)
        {
            _shelterRepository = shelterRepository;
            _petLikesService = petLikesService;
        }

        public async Task<IEnumerable<PetBasicShelterDto>> HandleAsync(GetLikedPets query)
        {
            List<PetBasicShelterDto> pets = new List<PetBasicShelterDto>();
            IMongoQueryable<ShelterPetDocument> queryable = _shelterRepository.Collection.AsQueryable();
            IEnumerable<Guid> likePetIds = await _petLikesService.GetUserLikedPetIdsAsync(query.UserId);

            foreach (Guid petId in likePetIds)
            {
                ShelterPetDocument pet = await queryable.FirstOrDefaultAsync(x => x.Id == petId);
                pets.Add(pet.AsBasicDto(query.Latitude, query.Longitude));
            }

            return pets;
        }
    }
}