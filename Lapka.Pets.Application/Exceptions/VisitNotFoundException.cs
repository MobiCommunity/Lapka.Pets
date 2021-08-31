namespace Lapka.Pets.Application.Exceptions
{
    public class VisitNotFoundException : AppException
    {
        public string Id { get; }
        public VisitNotFoundException(string id) : base($"Visit is not found: {id}")
        {
            Id = id;
        }

        public override string Code => "visit_not_found";
    }
}