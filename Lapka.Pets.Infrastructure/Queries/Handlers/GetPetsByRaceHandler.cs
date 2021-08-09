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
    public class GetPetsByRaceHandler : IQueryHandler<GetPetsByRace, IEnumerable<PetBasicDto>>
    {
        private readonly IPetQueryService _petQueryService;

        public GetPetsByRaceHandler(IPetQueryService petQueryService)
        {
            _petQueryService = petQueryService;
        }
        public async Task<IEnumerable<PetBasicDto>> HandleAsync(GetPetsByRace query)
        {
            if (string.IsNullOrEmpty(query.Race))
                return new List<PetBasicDto>();
            
            IEnumerable<Pet> pets = await _petQueryService.GetAllByRaceAsync(query.Race);
            
            return pets.Select(x => x.AsBasicDto());
        }
    }
}