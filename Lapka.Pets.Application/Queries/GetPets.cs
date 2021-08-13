using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto;

namespace Lapka.Pets.Application.Queries
{
    public class GetPets : IQuery<IEnumerable<PetBasicDto>>
    {
        public string Name { get; set; }
        public string Race { get; set; }
    }
}