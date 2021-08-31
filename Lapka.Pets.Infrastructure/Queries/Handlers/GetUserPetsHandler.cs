using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetUserPetsHandler : IQueryHandler<GetUserPets, IEnumerable<PetBasicUserDto>>
    {
        private readonly IMongoRepository<PetUserDocument, Guid> _repository;

        public GetUserPetsHandler(IMongoRepository<PetUserDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PetBasicUserDto>> HandleAsync(GetUserPets query)
        {
            IReadOnlyList<PetUserDocument> pets = await _repository.FindAsync(x => x.UserId == query.UserId);

            return pets.Select(x => x.AsBasicDto());
        }
    }
}