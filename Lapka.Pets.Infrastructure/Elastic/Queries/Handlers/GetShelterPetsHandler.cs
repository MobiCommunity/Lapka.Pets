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
    public class GetShelterPetsHandler : IQueryHandler<GetShelterPets, IEnumerable<PetBasicShelterDto>>
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetShelterPetsHandler(IQueryDispatcher queryDispatcher, IElasticClient elasticClient,
            ElasticSearchOptions elasticSearchOptions)
        {
            _queryDispatcher = queryDispatcher;
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<IEnumerable<PetBasicShelterDto>> HandleAsync(GetShelterPets query)
        {
            IEnumerable<PetBasicShelterDto> likedPets = await _queryDispatcher.QueryAsync(new GetLikedPets
            {
                UserId = query.UserId
            });
            
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
                        ,
                        new QueryContainer(new TermQuery
                        {
                            Field = Infer.Field<ShelterPetDocument>(pet => pet.Species),
                            Value = query.Species
                        })
                    }
                }),
            };

            ISearchResponse<ShelterPetDocument> search =
                await _elasticClient.SearchAsync<ShelterPetDocument>(searchRequest);

            IEnumerable<ShelterPetDocument> pets = search?.Documents;

            if (pets is null) return null;

            return pets.Select(x => x.AsBasicDto(query.Latitude, query.Longitude, likedPets.Any(p => p.Id == x.Id)));
        }
    }
}