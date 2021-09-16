using System;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Pets.Infrastructure.Pets
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