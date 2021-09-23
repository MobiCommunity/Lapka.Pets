﻿using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Queries.LostPets;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Elastic.Options;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using Lapka.Pets.Infrastructure.Options;
using Nest;

namespace Lapka.Pets.Infrastructure.Elastic.Queries.Handlers
{
    public class GetLostPetElasticHandler : IQueryHandler<GetLostPetElastic, PetDetailsLostDto>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetLostPetElasticHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<PetDetailsLostDto> HandleAsync(GetLostPetElastic query)
        {
            GetResponse<LostPetDocument> response = await _elasticClient.GetAsync(
                new DocumentPath<LostPetDocument>(new Id(query.Id.ToString())),
                x => x.Index(_elasticSearchOptions.Aliases.LostPets));

            LostPetDocument pet = response?.Source;
            if (pet is null || pet.IsDeleted)
            {
                throw new PetNotFoundException(query.Id);
            }
            
            return pet.AsDetailDto(query.Latitude, query.Longitude);
        }
    }
}