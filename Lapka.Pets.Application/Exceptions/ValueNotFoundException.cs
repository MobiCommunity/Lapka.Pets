namespace Lapka.Pets.Application.Exceptions
{
    public class ValueNotFoundException : AppException
    {
        public override string Code => "value_not_found";

        public ValueNotFoundException() : base("value not exists in database")
        {
            
        }

    }
}