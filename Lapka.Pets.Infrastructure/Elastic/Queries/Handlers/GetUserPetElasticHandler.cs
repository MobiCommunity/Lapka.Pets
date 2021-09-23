using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Queries.UserPets;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Elastic.Options;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using Lapka.Pets.Infrastructure.Options;
using Nest;

namespace Lapka.Pets.Infrastructure.Elastic.Queries.Handlers
{
    public class GetUserPetElasticHandler : IQueryHandler<GetUserPetElastic, PetDetailsUserDto>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetUserPetElasticHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<PetDetailsUserDto> HandleAsync(GetUserPetElastic query)
        {
            GetResponse<UserPetDocument> response = await _elasticClient.GetAsync(
                new DocumentPath<UserPetDocument>(new Id(query.Id.ToString())),
                x => x.Index(_elasticSearchOptions.Aliases.UsersPets));

            UserPetDocument pet = response?.Source;
            if (pet == null || pet.IsDeleted)
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