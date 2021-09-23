using System;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Services
{
    public interface IGrpcIdentityService
    {
         Task<ShelterBasicInfo> GetShelterBasicInfo(Guid shelterId);
    }
}