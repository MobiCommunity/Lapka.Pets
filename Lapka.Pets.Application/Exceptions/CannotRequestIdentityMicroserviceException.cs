using System;

namespace Lapka.Pets.Application.Exceptions
{
    public class CannotRequestIdentityMicroserviceException : AppException
    {
        public Exception Exception { get; }
        
        public CannotRequestIdentityMicroserviceException(Exception exception) 
            : base($"Cannot request identity microservice: {exception.Message}")
        {
            Exception = exception;
        }

        public override string Code => "cannot_request_identity_microservice";
    }
}