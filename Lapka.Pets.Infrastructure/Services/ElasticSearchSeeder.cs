using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Pets.Infrastructure.Services
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

            IReadOnlyList<UserPetDocument> usersPets = await _userPetsRepository.FindAsync(_ => true);
            IReadOnlyList<LostPetDocument> lostPets = await _lostPetRepository.FindAsync(_ => true);
            IReadOnlyList<LikePetDocument> likedPets = await _likedPetDocumentRepository.FindAsync(_ => true);


            BulkAllObservable<UserPetDocument> bulkUsersPets =
                _client.BulkAll(usersPets, b => b.Index(_elasticOptions.Aliases.UsersPets));
            BulkAllObservable<LostPetDocument> bulkLostPets =
                _client.BulkAll(lostPets, b => b.Index(_elasticOptions.Aliases.LostPets));
            BulkAllObservable<LikePetDocument> bulkLikedPets =
                _client.BulkAll(likedPets, b => b.Index(_elasticOptions.Aliases.LikedPets));


            bulkUsersPets.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("Users pets indexed"));
            bulkLostPets.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("Lost pets indexed"));
            bulkLikedPets.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("Liked pets indexed"));
        }

        private async Task SeedShelterPetsAsync()
        {
            IReadOnlyList<ShelterPetDocument> shelterPets = await _shelterPetRepository.FindAsync(_ => true);
            BulkAllObservable<ShelterPetDocument> bulkShelterPets =
                _client.BulkAll(shelterPets, b => b.Index(_elasticOptions.Aliases.ShelterPets));
            bulkShelterPets.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("Shelter pets indexed"));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}