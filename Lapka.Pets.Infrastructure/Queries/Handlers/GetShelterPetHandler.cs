using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetShelterPetHandler : IQueryHandler<GetShelterPet, PetDetailsShelterDto>
    {
        private readonly IMongoRepository<PetShelterDocument, Guid> _repository;

        public GetShelterPetHandler(IMongoRepository<PetShelterDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PetDetailsShelterDto> HandleAsync(GetShelterPet query)
        {
            PetShelterDocument pet = await PetHelpers.GetPetFromRepositoryAsync(_repository, query.Id);

            return pet.AsDetailDto(query.Latitude, query.Longitude);
        }
    }
}