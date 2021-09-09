namespace Lapka.Pets.Application.Exceptions
{
    public class InvalidShelterIdException : AppException
    {
        public string ShelterId { get; }
        public InvalidShelterIdException(string shelterId) : base($"Invalid shelter id: {shelterId}")
        {
            ShelterId = shelterId;
        }

        public override string Code => "invalid_shelter_id";
    }
}