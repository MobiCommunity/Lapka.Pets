using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Services;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetShelterPetHandler : IQueryHandler<GetShelterPet, PetDetailsShelterDto>
    {
        private readonly IPetRepository<ShelterPet> _repository;

        public GetShelterPetHandler(IPetRepository<ShelterPet> repository)
        {
            _repository = repository;
        }

        public async Task<PetDetailsShelterDto> HandleAsync(GetShelterPet query)
        {
            ShelterPet pet = await _repository.GetByIdAsync(query.Id);
            if (pet is null)
            {
                throw new PetNotFoundException(query.Id);
            }

            return pet.AsDetailDto(query.Latitude, query.Longitude);
        }
    }
}