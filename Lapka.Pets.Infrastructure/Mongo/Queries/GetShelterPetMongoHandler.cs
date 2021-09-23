using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries.ShelterPets;
using Lapka.Pets.Infrastructure.Elastic.Options;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Pets.Infrastructure.Mongo.Queries
{
    public class GetShelterPetMongoHandler : IQueryHandler<GetShelterPetMongo, PetDetailsShelterDto>
    {
        private readonly IMongoRepository<ShelterPetDocument, Guid> _repository;

        public GetShelterPetMongoHandler(IMongoRepository<ShelterPetDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PetDetailsShelterDto> HandleAsync(GetShelterPetMongo query)
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