using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries.ShelterPets;
using Lapka.Pets.Infrastructure.Elastic.Options;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Pets.Infrastructure.Elastic.Queries.Handlers
{
    public class GetShelterOwnPetsCountHandler : IQueryHandler<GetShelterOwnPetsCount, int>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetShelterOwnPetsCountHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }
        
        public async Task<int> HandleAsync(GetShelterOwnPetsCount query)
        {
            Func<QueryContainerDescriptor<ShelterPetDocument>, QueryContainer> searchQuery = q => new MatchQuery
            {
                Query = query.ShelterId.ToString(),
                Field = Infer.Field<ShelterPetDocument>(p => p.ShelterId)
            };

            CountResponse searchResult = await _elasticClient.CountAsync<ShelterPetDocument>(q =>
                q.Query(searchQuery).Index(_elasticSearchOptions.Aliases.UsersPets));

            if (!int.TryParse(searchResult.Count.ToString(), out Int32 petCount))
            {
                throw new CannotConvertShelterPetCountIntoIntException(searchResult.Count.ToString());
            };
            return petCount;
        }
    }
}