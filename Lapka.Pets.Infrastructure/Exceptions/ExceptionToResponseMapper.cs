using Convey.WebApi.Exceptions;
using System;
using System.Net;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Infrastructure.Exceptions
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                DomainException ex => new ExceptionResponse(new {code = ex.Code, reason = ex.Message},
                    HttpStatusCode.BadRequest),

                AppException ex => ex switch
                {
                    PetNotFoundException petNotFoundException => 
                        new ExceptionResponse (new
                        {
                            code = petNotFoundException.Code,
                            reason = petNotFoundException.Message
                        },HttpStatusCode.NotFound),
                    CannotRequestFilesMicroserviceException cannotRequestFilesMicroserviceException => 
                        new ExceptionResponse (new
                        {
                            code = cannotRequestFilesMicroserviceException.Code,
                            reason = cannotRequestFilesMicroserviceException.Message
                        },HttpStatusCode.InternalServerError),
                    PetsNotFoundException petsNotFoundException => 
                        new ExceptionResponse (new
                        {
                            code = petsNotFoundException.Code,
                            reason = petsNotFoundException.Message
                        },HttpStatusCode.NotFound),
                    _ => new ExceptionResponse(
                        new
                        {
                            code = ex.Code,
                            reason = ex.Message
                        },
                        HttpStatusCode.BadRequest)
                },

                _ => new ExceptionResponse(new
                    {
                        code = "error", reason = "There was an error."
                    },
                    HttpStatusCode.BadRequest)
            };
    }
}