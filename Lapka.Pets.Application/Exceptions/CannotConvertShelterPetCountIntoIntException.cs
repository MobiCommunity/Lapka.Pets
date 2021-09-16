namespace Lapka.Pets.Application.Exceptions
{
    public class CannotConvertShelterPetCountIntoIntException : AppException
    {
        public string Count { get; }

        public CannotConvertShelterPetCountIntoIntException(string count) : base(
            $"Cannot convert count {count} from string type to int type")
        {
            Count = count;
        }

        public override string Code => "could_not_parse_shelter_pets_count";
    }
}