﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Options;
using Lapka.Pets.Infrastructure.Services;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nest;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetLikedPetsHandler : IQueryHandler<GetLikedPets, IEnumerable<PetBasicShelterDto>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetLikedPetsHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<IEnumerable<PetBasicShelterDto>> HandleAsync(GetLikedPets query)
        {
            GetResponse<LikePetDocument> searchLikePets = await _elasticClient.GetAsync(
                new DocumentPath<LikePetDocument>(new Id(query.UserId.ToString())),
                x => x.Index(_elasticSearchOptions.Aliases.LikedPets));

            List<Id> petIds = GetPetIdsOutOfLikedPetsList(searchLikePets);

            if (petIds.Count() == 0)
            {
                return new List<PetBasicShelterDto>();
            }
            
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.ShelterPets)
            {
                Query = new IdsQuery
                {
                    Values = petIds
                }
            };

            ISearchResponse<ShelterPetDocument> searchShelterPets = await _elasticClient.SearchAsync<ShelterPetDocument>(searchRequest);

            return searchShelterPets?.Documents.Select(x => x.AsBasicDto(query.Latitude, query.Longitude));
        }

        private static List<Id> GetPetIdsOutOfLikedPetsList(GetResponse<LikePetDocument> searchLikePets)
        {
            List<Id> petIds = new List<Id>();
            if (searchLikePets?.Source.LikedPets is { })
            {
                foreach (Guid petId in searchLikePets?.Source.LikedPets)
                {
                    petIds.Add(petId);
                }
            }

            return petIds;
        }
    }
}