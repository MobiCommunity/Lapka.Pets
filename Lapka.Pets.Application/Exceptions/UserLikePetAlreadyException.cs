using System;

namespace Lapka.Pets.Application.Exceptions
{
    public class UserLikePetAlreadyException : AppException
    {
        public Guid UserId { get; }
        public Guid PetId { get; }
        public UserLikePetAlreadyException(Guid userId, Guid petId) : base($"User: {userId} already like pet {petId}")
        {
            UserId = userId;
            PetId = petId;
        }
        public override string Code => "user_already_like_pet";
    }
}