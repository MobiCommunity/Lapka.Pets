using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;

namespace Lapka.Pets.Application.Queries.LostPets
{
    public class GetLostPetPhotos : IQuery<IEnumerable<string>>
    {
        public Guid Id { get; set; }
    }
}