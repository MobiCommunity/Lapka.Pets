using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Queries
{
    public static class PetHelpers
    {
        public static async Task<TPet> GetPetFromRepositoryAsync<TPet>(IMongoRepository<TPet, Guid> petRepository,
            Guid petId) where TPet : PetDocument
        {
            TPet pet = await petRepository.GetAsync(petId);
            if (pet == null)
            {
                throw new PetNotFoundException(petId);
            }

            return pet;
        }
        
        public static void ValidIfUserIsOwnerOfPet(Guid userId, Guid petUserId)
        {
            if (petUserId != userId)
            {
                throw new PetDoesNotBelongToUserException(userId.ToString(), petUserId.ToString());
            }
        }
    }
}