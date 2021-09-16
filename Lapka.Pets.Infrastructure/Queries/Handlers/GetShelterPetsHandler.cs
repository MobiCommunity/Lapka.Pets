using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nest;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetShelterPetsHandler : IQueryHandler<GetShelterPets, IEnumerable<PetBasicShelterDto>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetShelterPetsHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<IEnumerable<PetBasicShelterDto>> HandleAsync(GetShelterPets query)
        {
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.ShelterPets)
            {
                Query = new QueryContainer(new BoolQuery
                {
                    Should = new List<QueryContainer>
                    {
                        new QueryContainer(new FuzzyQuery
                        {
                            Field = Infer.Field<ShelterPetDocument>(pet => pet.Race),
                            Value = query.Race,
                            Fuzziness = Fuzziness.AutoLength(3, 4)
                        }),
                        new QueryContainer(new FuzzyQuery
                        {
                            Field = Infer.Field<ShelterPetDocument>(pet => pet.Name),
                            Value = query.Name,
                            Fuzziness = Fuzziness.AutoLength(3, 4)
                        })
                    }
                }),
            };

            ISearchResponse<ShelterPetDocument> search =
                await _elasticClient.SearchAsync<ShelterPetDocument>(searchRequest);

            return search?.Documents.Select(x => x.AsBasicDto(query.Latitude, query.Longitude));
        }
    }
}