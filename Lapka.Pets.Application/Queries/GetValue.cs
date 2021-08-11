using Convey.CQRS.Queries;
using System;
using Lapka.Pets.Application.Dto;

namespace Lapka.Pets.Application.Queries
{
    public class GetValue : IQuery<ValueDto>
    {
        public Guid Id { get; set; }

    }
}
