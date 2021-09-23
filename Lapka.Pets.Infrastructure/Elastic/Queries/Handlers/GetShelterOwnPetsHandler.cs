using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Queries.ShelterPets;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Elastic.Options;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using Lapka.Pets.Infrastructure.Options;
using Nest;

namespace Lapka.Pets.Infrastructure.Elastic.Queries.Handlers
{
    public class GetShelterOwnPetsHandler : IQueryHandler<GetShelterOwnPets, IEnumerable<ShelterPetDto>>
    {      
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetShelterOwnPetsHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }
        
        public async Task<IEnumerable<ShelterPetDto>> HandleAsync(GetShelterOwnPets query)
        {
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.ShelterPets)
            {
                Query = new MatchQuery
                {
                    Query = query.ShelterId.ToString(),
                    Field = Infer.Field<ShelterPetDocument>(p => p.ShelterId)
                }
            };
            
            ISearchResponse<ShelterPetDocument> search = await _elasticClient.SearchAsync<ShelterPetDocument>(searchRequest);

            return search?.Documents.Select(x => x.AsShelterPetDto());
        }
    }
}