using System;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;

namespace Lapka.Pets.Application.Queries.ShelterPets
{
    public class GetShelterPetMongo : IQuery<PetDetailsShelterDto>
    {
        public Guid Id { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}