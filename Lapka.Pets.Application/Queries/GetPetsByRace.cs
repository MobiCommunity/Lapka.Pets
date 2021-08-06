using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto;

namespace Lapka.Pets.Application.Queries
{
    public class GetPetsByRace: IQuery<IEnumerable<PetBasicDto>>
    {
        public string Race { get; set; }
    }
}