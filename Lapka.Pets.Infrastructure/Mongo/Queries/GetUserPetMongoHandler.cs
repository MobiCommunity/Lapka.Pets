using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries.UserPets;
using Lapka.Pets.Infrastructure.Mongo.Documents;

namespace Lapka.Pets.Infrastructure.Mongo.Queries
{
    public class GetUserPetMongoHandler : IQueryHandler<GetUserPetMongo, PetDetailsUserDto>
    {
        private readonly IMongoRepository<UserPetDocument, Guid> _repository;


        public GetUserPetMongoHandler(IMongoRepository<UserPetDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PetDetailsUserDto> HandleAsync(GetUserPetMongo query)
        {
            UserPetDocument pet = await _repository.GetAsync(query.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(query.Id);
            }

            return pet.AsDetailsDto();
        }
    }
}