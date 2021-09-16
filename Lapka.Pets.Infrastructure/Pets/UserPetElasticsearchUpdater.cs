using System;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Pets.Infrastructure.Pets
{
    public class UserPetElasticsearchUpdater : IUserPetElasticsearchUpdater
    {
        private readonly ILogger<UserPetElasticsearchUpdater> _logger;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public UserPetElasticsearchUpdater(ILogger<UserPetElasticsearchUpdater> logger, IElasticClient elasticClient,
            ElasticSearchOptions elasticSearchOptions)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task InsertAndUpdateDataAsync(UserPet pet)
        {
            IndexResponse response = await _elasticClient.IndexAsync(pet.AsDocument(),
                x => x.Index(_elasticSearchOptions.Aliases.UsersPets));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to insert or update" +
                                 $" user pet {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }
        
        public async Task DeleteDataAsync(Guid petId)
        {
            DeleteResponse response = await _elasticClient.DeleteAsync<UserPet>(petId,
                x => x.Index(_elasticSearchOptions.Aliases.UsersPets));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to delete" +
                                 $" user pet {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }
    }
}