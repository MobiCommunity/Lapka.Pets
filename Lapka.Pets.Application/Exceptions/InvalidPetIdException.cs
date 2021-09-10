namespace Lapka.Pets.Application.Exceptions
{
    public class InvalidPetIdException : AppException
    {
        public string PetId { get; }
        public InvalidPetIdException(string petId) : base($"invalid pet id: {petId}")
        {
            PetId = petId;
        }
        public override string Code => "invalid_pet_id";
    }
}