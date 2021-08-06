namespace Lapka.Pets.Application.Exceptions
{
    public class ValueAlreadyExistsException : AppException
    {
        public override string Code => "Value_exists";

        public ValueAlreadyExistsException() : base("Value already in database")
        {
            
        }

    }
}