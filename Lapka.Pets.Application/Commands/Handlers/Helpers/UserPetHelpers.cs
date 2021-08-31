using System;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers.Helpers
{
    public static class UserPetHelpers
    {
        public static void ValidateUserAndPet(Guid userId, Guid petId, UserPet pet)
        {
            if (pet is null)
            {
                throw new PetNotFoundException(pet.Id.Value);
            }

            if (pet.UserId != userId)
            {
                throw new PetDoesNotBelongToUserException(userId.ToString(), pet.Id.ToString());
            }
        }
    }
}