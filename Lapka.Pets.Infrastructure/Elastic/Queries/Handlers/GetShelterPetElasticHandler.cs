using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Queries.ShelterPets;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Elastic.Options;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using Lapka.Pets.Infrastructure.Options;
using Nest;

namespace Lapka.Pets.Infrastructure.Elastic.Queries.Handlers
{
    public class GetShelterPetElasticHandler : IQueryHandler<GetShelterPetElastic, PetDetailsShelterDto>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetShelterPetElasticHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<PetDetailsShelterDto> HandleAsync(GetShelterPetElastic query)
        {
            GetResponse<ShelterPetDocument> response = await _elasticClient.GetAsync(
                new DocumentPath<ShelterPetDocument>(new Id(query.Id.ToString())),
                x => x.Index(_elasticSearchOptions.Aliases.ShelterPets));

            ShelterPetDocument pet = response?.Source;
            if (pet == null || pet.IsDeleted)
            {
                throw new PetNotFoundException(query.Id);
            }

            return pet.AsDetailDto(query.Latitude, query.Longitude);
        }
    }
}