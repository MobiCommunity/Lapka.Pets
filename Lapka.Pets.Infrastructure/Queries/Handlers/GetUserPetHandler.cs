using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetUserPetHandler : IQueryHandler<GetUserPet, PetDetailsUserDto>
    {
        private readonly IMongoRepository<PetUserDocument, Guid> _mongoRepository;

        public GetUserPetHandler(IMongoRepository<PetUserDocument, Guid> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<PetDetailsUserDto> HandleAsync(GetUserPet query)
        {
            PetUserDocument pet = await _mongoRepository.GetAsync(query.Id);
            if (pet is null)
            {
                throw new PetNotFoundException(query.Id);
            }

            return pet.AsBusiness().AsDetailsDto();
        }
    }
}