using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Queries.LostPets;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Elastic.Options;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using Lapka.Pets.Infrastructure.Options;
using Nest;

namespace Lapka.Pets.Infrastructure.Elastic.Queries.Handlers
{
    public class GetLostPetsHandler : IQueryHandler<GetLostPets, IEnumerable<PetBasicLostDto>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetLostPetsHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<IEnumerable<PetBasicLostDto>> HandleAsync(GetLostPets query)
        {
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.LostPets);

            ISearchResponse<LostPetDocument> search = await _elasticClient.SearchAsync<LostPetDocument>(searchRequest);

            return search?.Documents.Select(x => x.AsBasicDto(query.Latitude, query.Longitude)).OrderBy(x => x.Distance);
        }
    }
}