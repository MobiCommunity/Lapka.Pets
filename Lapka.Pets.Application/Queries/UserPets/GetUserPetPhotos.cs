using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;

namespace Lapka.Pets.Application.Queries.UserPets
{
    public class GetUserPetPhotos : IQuery<IEnumerable<string>>
    {
        public Guid Id { get; set; }
    }
}