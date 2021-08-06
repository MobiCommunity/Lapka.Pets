using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Dto;

namespace Lapka.Pets.Application.Services
{
    public interface IValueQueryService
    {
        Task<ValueDto> GetValueById(Guid id);
        Task<IEnumerable<ValueDto>> GetAllValues();
    }
}