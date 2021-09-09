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
    public class GetShelterOwnPetsHandler : IQueryHandler<GetShelterOwnPets, IEnumerable<PetBasicShelterDto>>
    {      
        private readonly IMongoRepository<ShelterPetDocument, Guid> _mongoRepository;

        public GetShelterOwnPetsHandler(IMongoRepository<ShelterPetDocument, Guid> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
        public async Task<IEnumerable<PetBasicShelterDto>> HandleAsync(GetShelterOwnPets query)
        {
            IReadOnlyList<ShelterPetDocument> pets = await _mongoRepository.FindAsync(x => x.ShelterId == query.ShelterId);

            return pets.Select(x => x.AsBasicDto(null, null));
        }
    }
}