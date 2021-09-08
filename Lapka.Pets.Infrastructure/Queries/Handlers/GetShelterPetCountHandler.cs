using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetShelterPetCountHandler : IQueryHandler<GetShelterPetCount, int>
    {
        private readonly IMongoRepository<ShelterPetDocument, Guid> _repository;

        public GetShelterPetCountHandler(IMongoRepository<ShelterPetDocument, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<int> HandleAsync(GetShelterPetCount query)
        {
            IMongoQueryable<ShelterPetDocument> queryPets = _repository.Collection.AsQueryable();
            return await queryPets.Where(x => x.ShelterId == query.ShelterId).CountAsync();
        }
    }
}