using System;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;

namespace Lapka.Pets.Application.Queries.LostPets
{
    public class GetLostPetMongo : IQuery<PetDetailsLostDto>
    {
        public Guid Id { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}