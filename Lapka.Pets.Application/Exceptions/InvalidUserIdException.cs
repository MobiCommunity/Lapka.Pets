using System;

namespace Lapka.Pets.Application.Exceptions
{
    public class InvalidUserIdException : AppException
    {
        public string UserId { get; }
        public InvalidUserIdException(string userId) : base($"User id is not valid: {userId}")
        {
            UserId = userId;
        }
        public override string Code => "user_id_is_not_valid";
    }
}