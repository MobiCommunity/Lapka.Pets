namespace Lapka.Pets.Application.Exceptions
{
    public class PetsNotFoundException : AppException
    {
        public string Race { get; }
        public PetsNotFoundException(string race) : base($"Pets with this race not found: {race}")
        {
            Race = race;
        }

        public override string Code => "pets_not_found";
    }
}