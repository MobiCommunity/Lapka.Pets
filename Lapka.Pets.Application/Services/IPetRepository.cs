using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services
{
    public interface IPetRepository
    {
        Task AddAsync(Pet pet);
        Task DeleteAsync(Pet pet);
        Task UpdateAsync(Pet pet);
    }
}