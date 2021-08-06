using System;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto;

namespace Lapka.Pets.Application.Queries
{
    public class GetPet : IQuery<PetDetailsDto>
    {
        public Guid Id { get; set; }
    }
}