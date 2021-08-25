using System;
using System.Threading.Tasks;

namespace Lapka.Pets.Application.Services
{
    public interface IGrpcPetService
    {
        Task AddPet(Guid userId, Guid petId);
        Task DeletePet(Guid userId, Guid petId);
    }
}