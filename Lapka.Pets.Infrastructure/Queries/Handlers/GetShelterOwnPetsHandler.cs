using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Options;
using Nest;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetShelterOwnPetsHandler : IQueryHandler<GetShelterOwnPets, IEnumerable<PetBasicShelterDto>>
    {      
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetShelterOwnPetsHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }
        
        public async Task<IEnumerable<PetBasicShelterDto>> HandleAsync(GetShelterOwnPets query)
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

            return search?.Documents.Select(x => x.AsBasicDto(null, null));
        }
    }
}