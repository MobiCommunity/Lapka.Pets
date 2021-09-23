using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure.Elastic.Options;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Pets.Infrastructure.Elastic.Services
{
    public class PetLikeElasticsearchUpdater : IPetLikeElasticsearchUpdater
    {
        private readonly ILogger<PetLikeElasticsearchUpdater> _logger;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public PetLikeElasticsearchUpdater(ILogger<PetLikeElasticsearchUpdater> logger, IElasticClient elasticClient,
            ElasticSearchOptions elasticSearchOptions)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task InsertAndUpdateDataAsync(UserLikedPets likedPets)
        {
            IndexResponse response = await _elasticClient.IndexAsync(likedPets.AsDocument(),
                x => x.Index(_elasticSearchOptions.Aliases.LikedPets));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to insert or update" +
                                 $" like pet {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }
        
        public async Task DeleteDataAsync(UserLikedPets likedPets)
        {
            DeleteResponse response = await _elasticClient.DeleteAsync<LikePetDocument>(likedPets.AsDocument(),
                x => x.Index(_elasticSearchOptions.Aliases.LikedPets));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to delete" +
                                 $" like pet {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }
    }
}