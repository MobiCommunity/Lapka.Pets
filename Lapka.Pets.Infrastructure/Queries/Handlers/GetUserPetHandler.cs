using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetUserPetHandler : IQueryHandler<GetUserPet, PetDetailsUserDto>
    {
        private readonly IMongoRepository<UserPetDocument, Guid> _repository;

        public GetUserPetHandler(IMongoRepository<UserPetDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PetDetailsUserDto> HandleAsync(GetUserPet query)
        {
            UserPetDocument pet = await PetHelpers.GetPetFromRepositoryAsync(_repository, query.Id);
            PetHelpers.ValidIfUserIsOwnerOfPet(query.UserId, pet.UserId);

            return pet.AsDetailsDto();
        }
    }
}