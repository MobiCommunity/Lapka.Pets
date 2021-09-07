using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetShelterPetsHandler : IQueryHandler<GetShelterPets, IEnumerable<PetBasicShelterDto>>
    {
        private readonly IMongoRepository<ShelterPetDocument, Guid> _mongoRepository;

        public GetShelterPetsHandler(IMongoRepository<ShelterPetDocument, Guid> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<IEnumerable<PetBasicShelterDto>> HandleAsync(GetShelterPets query)
        {
            IMongoQueryable<ShelterPetDocument> queryable = _mongoRepository.Collection.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                queryable = queryable.Where(x => x.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.Race))
            {
                queryable = queryable.Where(x => x.Race.Contains(query.Race));
            }

            IList<ShelterPetDocument> search = await queryable.ToListAsync();

            return search.Select(x => x.AsBasicDto(query.Latitude, query.Longitude));
        }
    }
}