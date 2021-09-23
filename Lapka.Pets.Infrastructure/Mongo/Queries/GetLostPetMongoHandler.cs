using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries.LostPets;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure.Elastic.Options;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Pets.Infrastructure.Mongo.Queries
{
    public class GetLostPetMongoHandler : IQueryHandler<GetLostPetMongo, PetDetailsLostDto>
    {
        private readonly IMongoRepository<LostPetDocument, Guid> _repository;
        
        public GetLostPetMongoHandler(IMongoRepository<LostPetDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PetDetailsLostDto> HandleAsync(GetLostPetMongo query)
        {
            LostPetDocument pet = await _repository.GetAsync(query.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(query.Id);
            }
            
            return pet.AsDetailDto(query.Latitude, query.Longitude);
        }
    }
}