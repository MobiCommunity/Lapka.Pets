using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;

namespace Lapka.Pets.Application.Queries
{
    public class GetShelterOwnPets : IQuery<IEnumerable<PetBasicShelterDto>>
    {
        public Guid ShelterId { get; set; }
    }
}