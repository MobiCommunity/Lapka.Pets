using System;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Pets.Infrastructure.Pets
{
    public class LostPetElasticsearchUpdater : ILostPetElasticsearchUpdater
    {
        private readonly ILogger<LostPetElasticsearchUpdater> _logger;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public LostPetElasticsearchUpdater(ILogger<LostPetElasticsearchUpdater> logger, IElasticClient elasticClient,
            ElasticSearchOptions elasticSearchOptions)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task InsertAndUpdateDataAsync(LostPet pet)
        {
            IndexResponse response = await _elasticClient.IndexAsync(pet.AsDocument(),
                x => x.Index(_elasticSearchOptions.Aliases.LostPets));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to insert or update" +
                                 $" lost pet {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }
        
        public async Task DeleteDataAsync(Guid petId)
        {
            DeleteResponse response = await _elasticClient.DeleteAsync<LostPet>(petId,
                x => x.Index(_elasticSearchOptions.Aliases.LostPets));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to delete" +
                                 $" lost pet {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }
    }
}