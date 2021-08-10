using System;
using System.IO;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetPetHandler : IQueryHandler<GetPet, PetDetailsDto>
    {
        private readonly IMongoRepository<PetDocument, Guid> _mongoRepository;

        public GetPetHandler(IMongoRepository<PetDocument, Guid> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
        public async Task<PetDetailsDto> HandleAsync(GetPet query)
        {
            PetDocument pet = await _mongoRepository.GetAsync(query.Id);
            if (pet == null) throw new PetNotFoundException(query.Id);
            
            return pet.AsDetailDto();
        }
    }
}