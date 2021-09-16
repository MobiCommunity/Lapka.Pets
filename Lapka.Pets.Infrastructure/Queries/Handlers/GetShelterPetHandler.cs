using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Options;
using Nest;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetShelterPetHandler : IQueryHandler<GetShelterPet, PetDetailsShelterDto>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetShelterPetHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<PetDetailsShelterDto> HandleAsync(GetShelterPet query)
        {
            GetResponse<ShelterPetDocument> response = await _elasticClient.GetAsync(
                new DocumentPath<ShelterPetDocument>(new Id(query.Id.ToString())),
                x => x.Index(_elasticSearchOptions.Aliases.ShelterPets));

            ShelterPetDocument pet = response?.Source;
            if (pet == null)
            {
                throw new PetNotFoundException(query.Id);
            }

            return pet.AsDetailDto(query.Latitude, query.Longitude);
        }
    }
}