namespace Lapka.Pets.Application.Exceptions
{
    public class PetNotFoundException : AppException
    {
        public override string Code => "pet_not_found";

        public PetNotFoundException() : base("Pet not exists in database")
        {
            
        }

    }
}