using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nest;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
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