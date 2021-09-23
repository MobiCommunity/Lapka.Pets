using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Queries.UserPets;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Elastic.Options;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using Lapka.Pets.Infrastructure.Options;
using Nest;

namespace Lapka.Pets.Infrastructure.Elastic.Queries.Handlers
{
    public class GetUserPetsHandler : IQueryHandler<GetUserPets, IEnumerable<PetBasicUserDto>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetUserPetsHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<IEnumerable<PetBasicUserDto>> HandleAsync(GetUserPets query)
        {
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.UsersPets)
            {
                Query = new MatchQuery
                {
                    Query = query.UserId.ToString(),
                    Field = Infer.Field<UserPetDocument>(p => p.UserId == query.UserId)
                }
            };

            ISearchResponse<UserPetDocument> search =
                await _elasticClient.SearchAsync<UserPetDocument>(searchRequest);
            
            return search?.Documents.Select(x => x.AsBasicDto());
        }
    }
}