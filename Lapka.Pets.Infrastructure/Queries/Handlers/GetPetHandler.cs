using System;
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
    public class GetPetHandler : IQueryHandler<GetPet, PetDetailsDto>
    {
        private readonly IMongoRepository<PetDocument, Guid> _mongoRepository;

        public GetPetHandler(IMongoRepository<PetDocument, Guid> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<PetDetailsDto> HandleAsync(GetPet query)
        {
            PetDocument pet = await _mongoRepository.GetAsync(query.Id);
            if (pet is null)
            {
                throw new PetNotFoundException(query.Id);
            }

            return pet.AsDetailDto(query.Latitude, query.Longitude);
        }
    }
}