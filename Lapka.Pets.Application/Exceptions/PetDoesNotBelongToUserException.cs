namespace Lapka.Pets.Application.Exceptions
{
    public class PetDoesNotBelongToUserException : AppException
    {
        public string UserId { get; }
        public string PetId { get; }
        public PetDoesNotBelongToUserException(string userId, string petId) : base($"Pet: {petId}, does not belong to user: {userId}")
        {
            UserId = userId;
            PetId = petId;
        }

        public override string Code => "pet_does_not_belong_to_user";
    }
}