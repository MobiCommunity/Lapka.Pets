using System;
using System.Threading.Tasks;

namespace Lapka.Pets.Application.Services
{
    public interface IGrpcIdentityService
    {
        Task<bool> IsUserOwnerOfShelter(Guid shelterId, Guid userId);
        Task<Guid> ClosestShelter(string longitude, string latitude);
    }
}