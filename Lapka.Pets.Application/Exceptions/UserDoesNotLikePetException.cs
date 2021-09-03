using System;

namespace Lapka.Pets.Application.Exceptions
{
    public class UserDoesNotLikePetException : AppException
    {
        public Guid UserId { get; }
        public Guid PetId { get; }
        public UserDoesNotLikePetException(Guid userId, Guid petId) : base($"User: {userId} does not like pet {petId}")
        {
            UserId = userId;
            PetId = petId;
        }
        public override string Code => "user_does_not_like_pet";
    }
}