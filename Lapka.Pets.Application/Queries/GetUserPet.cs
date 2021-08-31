using System;
using Convey.CQRS.Queries;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Queries
{
    public class GetUserPet : IQuery<PetDetailsUserDto>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}