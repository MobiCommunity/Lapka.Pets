using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;

namespace Lapka.Pets.Application.Queries
{
    public class GetLikedPets : IQuery<IEnumerable<PetBasicShelterDto>>
    {
        public Guid UserId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}