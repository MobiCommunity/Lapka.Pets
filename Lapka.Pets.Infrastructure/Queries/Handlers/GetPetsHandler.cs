using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetPetsHandler : IQueryHandler<GetPets, IEnumerable<PetBasicDto>>
    {
        private readonly IMongoRepository<PetDocument, Guid> _mongoRepository;

        public GetPetsHandler(IMongoRepository<PetDocument, Guid> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<IEnumerable<PetBasicDto>> HandleAsync(GetPets query)
        {
            IMongoQueryable<PetDocument> queryable = _mongoRepository.Collection.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                queryable = queryable.Where(x => x.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.Race))
            {
                queryable = queryable.Where(x => x.Race.Contains(query.Race));
            }

            IList<PetDocument> search = await queryable.ToListAsync();

            return search.Select(x => x.AsBasicDto(query.Latitude, query.Longitude));
        }
    }
}