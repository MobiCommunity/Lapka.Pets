using System;
using Convey.CQRS.Queries;

namespace Lapka.Pets.Application.Queries
{
    public class GetShelterPetCount : IQuery<int>
    {
        public Guid ShelterId { get; set; }
    }
}