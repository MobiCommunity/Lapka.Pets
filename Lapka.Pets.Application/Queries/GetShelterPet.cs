using System;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Queries
{
    public class GetShelterPet : IQuery<PetDetailsShelterDto>
    {
        public Guid Id { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}