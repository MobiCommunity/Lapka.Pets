using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetUserPetsHandler : IQueryHandler<GetUserPets, IEnumerable<PetBasicUserDto>>
    {
        private readonly IMongoRepository<PetUserDocument, Guid> _mongoRepository;

        public GetUserPetsHandler(IMongoRepository<PetUserDocument, Guid> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<IEnumerable<PetBasicUserDto>> HandleAsync(GetUserPets query)
        {
            IReadOnlyList<PetUserDocument> pets = await _mongoRepository.FindAsync(_ => true);
            
            return pets.Select(x => x.AsBusiness().AsBasicDto());
        }
    }
}