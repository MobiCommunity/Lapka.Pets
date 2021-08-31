using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.PetDtos;

namespace Lapka.Pets.Application.Queries
{
    public class GetLostPets : IQuery<IEnumerable<PetBasicLostDto>>
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}