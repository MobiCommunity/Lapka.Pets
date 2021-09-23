using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;

namespace Lapka.Pets.Application.Queries.ShelterPets
{
    public class GetShelterOwnPets : IQuery<IEnumerable<ShelterPetDto>>
    {
        public Guid ShelterId { get; set; }
    }
}