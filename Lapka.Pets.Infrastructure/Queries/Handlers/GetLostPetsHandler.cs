using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetLostPetsHandler : IQueryHandler<GetLostPets, IEnumerable<PetBasicLostDto>>
    {
        private readonly IMongoRepository<LostPetDocument, Guid> _repository;

        public GetLostPetsHandler(IMongoRepository<LostPetDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PetBasicLostDto>> HandleAsync(GetLostPets query)
        {
            IReadOnlyList<LostPetDocument> pets = await _repository.FindAsync(_ => true);

            return pets.Select(x => x.AsBasicDto(query.Latitude, query.Longitude)).OrderBy(x => x.Distance);
        }
    }
}