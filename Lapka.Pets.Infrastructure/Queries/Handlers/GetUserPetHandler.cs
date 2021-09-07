using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;
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
            UserPetDocument pet = await _repository.GetAsync(query.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(query.Id);
            }

            if (pet.UserId != query.UserId)
            {
                throw new PetDoesNotBelongToUserException(query.UserId.ToString(), pet.UserId.ToString());
            }

            return pet.AsDetailsDto();
        }
    }
}