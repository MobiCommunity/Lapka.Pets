using System;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.PetDtos;

namespace Lapka.Pets.Application.Queries
{
    public class GetLostPet : IQuery<PetDetailsLostDto>
    {
        public Guid Id { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}