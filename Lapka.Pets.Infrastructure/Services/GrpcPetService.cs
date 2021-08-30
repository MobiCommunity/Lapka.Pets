using System;
using System.IO;
using System.Threading.Tasks;

namespace Lapka.Pets.Application.Services
{
    public class GrpcPetService : IGrpcPetService
    {
        private readonly PetGrpc.PetGrpcClient _client;

        public GrpcPetService(PetGrpc.PetGrpcClient client)
        {
            _client = client;
        }

        public async Task AddPetAsync(Guid userId, Guid petId)
        {
            await _client.AddPetToUserAsync(new UploadUserPetRequest
            {
                UserId = userId.ToString(),
                PetId = petId.ToString()
            });
        }

        public async Task DeletePetAsync(Guid userId, Guid petId)
        {
            await _client.DeletePetFromUserAsync(new DeleteUserPetRequest
            {
                UserId = userId.ToString(),
                PetId = petId.ToString()
            });        
        }
    }
}