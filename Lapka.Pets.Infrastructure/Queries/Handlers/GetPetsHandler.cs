using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Queries;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Infrastructure.Queries.Handlers
{
    public class GetPetsHandler : IQueryHandler<GetPets, IEnumerable<PetBasicDto>>
    {
        private readonly IPetQueryService _petQueryService;

        public GetPetsHandler(IPetQueryService petQueryService)
        {
            _petQueryService = petQueryService;
        }

        public async Task<IEnumerable<PetBasicDto>> HandleAsync(GetPets query)
        {
            IEnumerable<Pet> pets = await _petQueryService.GetAllAsync();
            
            return pets.Select(x => x.AsBasicDto());
            
        }
    }
}