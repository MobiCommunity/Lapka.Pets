using System;
using System.Threading.Tasks;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Infrastructure.Grpc.Services
{
    public class GrpcIdentityService : IGrpcIdentityService
    {
        private readonly ShelterProto.ShelterProtoClient _client;

        public GrpcIdentityService(ShelterProto.ShelterProtoClient client)
        {
            _client = client;
        }

        public async Task<ShelterBasicInfo> GetShelterBasicInfo(Guid shelterId)
        {
            GetShelterBasicInfoReply response = await _client.GetShelterBasicInfoAsync(new GetShelterBasicInfoRequest
            {
                ShelterId = shelterId.ToString()
            });
            
            return new ShelterBasicInfo(response.Name, new Location(response.Latitude, response.Longitude),
                new Address(response.Name, response.City, response.Street));
        }
    }
}