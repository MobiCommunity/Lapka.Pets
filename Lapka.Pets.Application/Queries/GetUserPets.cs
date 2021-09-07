using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;

namespace Lapka.Pets.Application.Queries
{
    public class GetUserPets : IQuery<IEnumerable<PetBasicUserDto>>
    {
        public Guid UserId { get; set; }
    }
}