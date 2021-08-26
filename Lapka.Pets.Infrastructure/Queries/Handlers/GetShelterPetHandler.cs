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
    public class GetShelterPetHandler : IQueryHandler<GetShelterPet, PetDetailsShelterDto>
    {
        private readonly IMongoRepository<PetShelterDocument, Guid> _mongoRepository;

        public GetShelterPetHandler(IMongoRepository<PetShelterDocument, Guid> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<PetDetailsShelterDto> HandleAsync(GetShelterPet query)
        {
            PetShelterDocument pet = await _mongoRepository.GetAsync(query.Id);
            if (pet is null)
            {
                throw new PetNotFoundException(query.Id);
            }

            return pet.AsDetailDto(query.Latitude, query.Longitude);
        }
    }
}