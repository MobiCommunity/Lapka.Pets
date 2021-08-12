using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Infrastructure.Services
{
    public class ValueRepository : IValueRepository
    {
        private readonly IList<Value> _values;

        public ValueRepository()
        {
            _values = new List<Value>();
        }

#pragma warning disable 1998
        public async Task AddValue(Value value)
#pragma warning restore 1998
        {
            _values.Add(value);
        }
#pragma warning disable 1998
        public async Task<ValueDto> GetById(Guid id)
#pragma warning restore 1998
        {
            var entity = _values.FirstOrDefault(x => x.Id.Value == id);
            if (entity is null) throw new ValueNotFoundException();
            return new ValueDto { Id = entity.Id.Value, Description = entity.Description, Name = entity.Name };
        }
    }
}