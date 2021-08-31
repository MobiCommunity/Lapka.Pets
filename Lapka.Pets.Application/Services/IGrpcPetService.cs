using System;
using System.Threading.Tasks;

namespace Lapka.Pets.Application.Services
{
    public interface IGrpcPetService
    {
        Task AddPetAsync(Guid userId, Guid petId);
        Task DeletePetAsync(Guid userId, Guid petId);
    }
}