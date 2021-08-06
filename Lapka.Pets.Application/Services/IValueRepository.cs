using System;
using System.Threading.Tasks;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services
{
    public interface IValueRepository
    {
        Task AddValue(Value value);
        Task<ValueDto> GetById(Guid id);
    }
}