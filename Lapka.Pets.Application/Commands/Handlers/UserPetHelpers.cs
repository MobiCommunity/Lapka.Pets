using System;
using System.Threading.Tasks;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers.Helpers
{
    public static class UserPetHelpers
    {
        public static async Task<UserPet> GetUserPetWithValidation(IPetRepository<UserPet> petRepository, Guid petId, Guid userId)
        {
            UserPet pet = await PetHelpers.GetPetFromRepositoryAsync(petRepository, petId);
            ValidIfUserIsOwnerOfPet(userId, pet.UserId);
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