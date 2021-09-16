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
using Lapka.Pets.Infrastructure.Options;
using Nest;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetUserPetHandler : IQueryHandler<GetUserPet, PetDetailsUserDto>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetUserPetHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<PetDetailsUserDto> HandleAsync(GetUserPet query)
        {
            GetResponse<UserPetDocument> response = await _elasticClient.GetAsync(
                new DocumentPath<UserPetDocument>(new Id(query.Id.ToString())),
                x => x.Index(_elasticSearchOptions.Aliases.UsersPets));

            UserPetDocument pet = response?.Source;
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