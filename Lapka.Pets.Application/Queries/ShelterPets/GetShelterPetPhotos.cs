using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;

namespace Lapka.Pets.Application.Queries.ShelterPets
{
    public class GetShelterPetPhotos : IQuery<IEnumerable<string>>
    {
        public Guid Id { get; set; }
    }
}