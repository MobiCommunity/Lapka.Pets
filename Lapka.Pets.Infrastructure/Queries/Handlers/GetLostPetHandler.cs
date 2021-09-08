﻿using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetLostPetHandler : IQueryHandler<GetLostPet, PetDetailsLostDto>
    {
        private readonly IMongoRepository<LostPetDocument, Guid> _repository;

        public GetLostPetHandler(IMongoRepository<LostPetDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PetDetailsLostDto> HandleAsync(GetLostPet query)
        {
            LostPetDocument pet = await _repository.GetAsync(query.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(query.Id);
            }
            
            return pet.AsDetailDto(query.Latitude, query.Longitude);
        }
    }
}