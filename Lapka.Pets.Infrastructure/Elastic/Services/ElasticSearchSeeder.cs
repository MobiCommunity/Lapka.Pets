using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Infrastructure.Elastic.Options;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Pets.Infrastructure.Elastic.Services
{
    public class ElasticSearchSeeder : IHostedService
    {
        private readonly ILogger<ElasticSearchSeeder> _logger;
        private readonly IMongoRepository<ShelterPetDocument, Guid> _shelterPetRepository;
        private readonly IMongoRepository<UserPetDocument, Guid> _userPetsRepository;
        private readonly IMongoRepository<LostPetDocument, Guid> _lostPetRepository;
        private readonly IMongoRepository<LikePetDocument, Guid> _likedPetDocumentRepository;
        private readonly IElasticClient _client;
        private readonly ElasticSearchOptions _elasticOptions;

        public ElasticSearchSeeder(ILogger<ElasticSearchSeeder> logger,
            IMongoRepository<ShelterPetDocument, Guid> shelterPetRepository,
            IMongoRepository<UserPetDocument, Guid> userPetsRepository,
            IMongoRepository<LostPetDocument, Guid> lostPetRepository,
            IMongoRepository<LikePetDocument, Guid> likedPetDocumentRepository, IElasticClient client,
            ElasticSearchOptions elasticOptions)
        {
            _logger = logger;
            _shelterPetRepository = shelterPetRepository;
            _userPetsRepository = userPetsRepository;
            _lostPetRepository = lostPetRepository;
            _likedPetDocumentRepository = likedPetDocumentRepository;
            _client = client;
            _elasticOptions = elasticOptions;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await SeedShelterPetsAsync();
            await SeedUserPetsAsync();
            await SeedLostPetsAsync();
            await SeedLikedPetsAsync();
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        private async Task SeedLikedPetsAsync()
        {
            IReadOnlyList<LikePetDocument> likedPets = await _likedPetDocumentRepository.FindAsync(_ => true);
            
            BulkAllObservable<LikePetDocument> bulkLikedPets =
                _client.BulkAll(likedPets, b => b.Index(_elasticOptions.Aliases.LikedPets));
            
            bulkLikedPets.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("Liked pets indexed"));
        }

        private async Task SeedLostPetsAsync()
        {
            IReadOnlyList<LostPetDocument> lostPets = await _lostPetRepository.FindAsync(x => !x.IsDeleted);
            
            await _client.Indices.DeleteAsync(_elasticOptions.Aliases.LostPets);

            BulkAllObservable<LostPetDocument> bulkLostPets =
                _client.BulkAll(lostPets, b => b.Index(_elasticOptions.Aliases.LostPets));
            
            bulkLostPets.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("Lost pets indexed"));
        }

        private async Task SeedUserPetsAsync()
        {
            IReadOnlyList<UserPetDocument> usersPets = await _userPetsRepository.FindAsync(x => !x.IsDeleted);
            
            await _client.Indices.DeleteAsync(_elasticOptions.Aliases.UsersPets);

            BulkAllObservable<UserPetDocument> bulkUsersPets =
                _client.BulkAll(usersPets, b => b.Index(_elasticOptions.Aliases.UsersPets));
            
            bulkUsersPets.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("Users pets indexed"));
        }

        private async Task SeedShelterPetsAsync()
        {
            IReadOnlyList<ShelterPetDocument> shelterPets = await _shelterPetRepository.FindAsync(x => !x.IsDeleted);
            
            await _client.Indices.DeleteAsync(_elasticOptions.Aliases.ShelterPets);

            BulkAllObservable<ShelterPetDocument> bulkShelterPets =
                _client.BulkAll(shelterPets, b => b.Index(_elasticOptions.Aliases.ShelterPets));
            
            bulkShelterPets.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("Shelter pets indexed"));
        }
    }
}