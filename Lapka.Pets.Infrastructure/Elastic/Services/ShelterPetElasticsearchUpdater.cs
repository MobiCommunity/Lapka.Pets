using System;
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
    public class ShelterPetElasticsearchUpdater : IShelterPetElasticsearchUpdater
    {
        private readonly ILogger<ShelterPetElasticsearchUpdater> _logger;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public ShelterPetElasticsearchUpdater(ILogger<ShelterPetElasticsearchUpdater> logger, IElasticClient elasticClient,
            ElasticSearchOptions elasticSearchOptions)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task InsertAndUpdateDataAsync(ShelterPet pet)
        {
            IndexResponse response = await _elasticClient.IndexAsync(pet.AsDocument(),
                x => x.Index(_elasticSearchOptions.Aliases.ShelterPets));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to insert or update" +
                                 $" shelter pet {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }
        
        public async Task DeleteDataAsync(Guid petId)
        {
            DeleteResponse response = await _elasticClient.DeleteAsync<ShelterPet>(petId,
                x => x.Index(_elasticSearchOptions.Aliases.ShelterPets));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to delete" +
                                 $" shelter pet {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }
    }
}