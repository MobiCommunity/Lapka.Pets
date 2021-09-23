using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;

namespace Lapka.Pets.Application.Queries.ShelterPets
{
    public class GetShelterPets : IQuery<IEnumerable<PetBasicShelterDto>>
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
    }
}