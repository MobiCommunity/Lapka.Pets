using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetPetsByRaceHandler : IQueryHandler<GetPetsByRace, IEnumerable<PetBasicDto>>
    {
        private readonly IMongoRepository<PetDocument, Guid> _mongoRepository;

        public GetPetsByRaceHandler(IMongoRepository<PetDocument, Guid> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<IEnumerable<PetBasicDto>> HandleAsync(GetPetsByRace query)
        {
            if (string.IsNullOrEmpty(query.Race))
                throw new PetsNotFoundException(query.Race);

            IEnumerable<PetDocument> pets = await _mongoRepository.FindAsync(x =>
                x.Race.ToUpper() == query.Race.ToUpper());

            if (string.IsNullOrEmpty(query.Latitude) || string.IsNullOrEmpty(query.Longitude))
            {
                return pets.Select(x => x.AsBasicDto());
            }
            
            Location location = new Location(query.Latitude, query.Longitude);
            return pets.Select(x => x.AsBasicDto(location));
        }
    }
}