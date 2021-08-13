namespace Lapka.Pets.Application.Exceptions
{
    public class CannotRequestFilesMicroserviceException : AppException
    {
        public string ErrorMessage { get; }
        
        public CannotRequestFilesMicroserviceException(string errorMessage) 
            : base($"Cannot request files microservice: {errorMessage}")
        {
            ErrorMessage = errorMessage;
        }

        public override string Code => "cannot_request_files_microservice";
    }
}