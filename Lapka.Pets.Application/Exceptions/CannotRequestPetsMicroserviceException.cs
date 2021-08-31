using System;

namespace Lapka.Pets.Application.Exceptions
{
    public class CannotRequestPetsMicroserviceException : AppException
    {
        public Exception Exception { get; }
        
        public CannotRequestPetsMicroserviceException(Exception exception) 
            : base($"Cannot request pets microservice: {exception.Message}")
        {
            Exception = exception;
        }

        public override string Code => "cannot_request_pets_microservice";
    }
}