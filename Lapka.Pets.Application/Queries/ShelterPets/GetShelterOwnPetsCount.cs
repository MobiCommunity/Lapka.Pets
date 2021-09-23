using System;
using Convey.CQRS.Queries;

namespace Lapka.Pets.Application.Queries.ShelterPets
{
    public class GetShelterOwnPetsCount : IQuery<int>
    {
        public Guid ShelterId { get; set; }
    }
}