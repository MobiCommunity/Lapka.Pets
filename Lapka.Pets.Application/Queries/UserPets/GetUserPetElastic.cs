using System;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto.Pets;

namespace Lapka.Pets.Application.Queries.UserPets
{
    public class GetUserPetElastic : IQuery<PetDetailsUserDto>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}