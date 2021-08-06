using System.IO;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetPetHandler : IQueryHandler<GetPet, PetDetailsDto>
    {
        private readonly IPetQueryService _petQueryService;

        public GetPetHandler(IPetQueryService petQueryService)
        {
            _petQueryService = petQueryService;
        }
        public async Task<PetDetailsDto> HandleAsync(GetPet query)
        {
            Pet pet = await _petQueryService.GetPetByIdAsync(query.Id);
            if (pet == null) throw new PetNotFoundException();
            
            return pet.AsDetailsDto();
        }
    }
}