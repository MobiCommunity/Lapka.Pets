using System;

namespace Lapka.Pets.Application.Exceptions
{
    public class UserNotOwnerOfShelterException : AppException
    {
        public Guid UserId { get; }
        public Guid ShelterId { get; }

        public UserNotOwnerOfShelterException(Guid userId, Guid shelterId) : base(
            $"User {userId} is not owner of {shelterId}")
        {
            UserId = userId;
            ShelterId = shelterId;
        }

        public override string Code => "user_not_owner_of_shelter";
    }
}