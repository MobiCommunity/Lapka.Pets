using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetShelterPetHandler : IQueryHandler<GetShelterPet, PetDetailsShelterDto>
    {
        private readonly IMongoRepository<ShelterPetDocument, Guid> _repository;

        public GetShelterPetHandler(IMongoRepository<ShelterPetDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PetDetailsShelterDto> HandleAsync(GetShelterPet query)
        {
            ShelterPetDocument pet = await _repository.GetAsync(query.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(query.Id);
            }
            
            return pet.AsDetailDto(query.Latitude, query.Longitude);
        }
    }
}