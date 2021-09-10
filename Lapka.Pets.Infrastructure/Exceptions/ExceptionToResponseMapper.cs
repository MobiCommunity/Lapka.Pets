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
                    InvalidPetIdException invalidPetIdException => 
                        new ExceptionResponse (new
                        {
                            code = invalidPetIdException.Code,
                            reason = invalidPetIdException.Message
                        },HttpStatusCode.BadRequest),
                    CannotRequestFilesMicroserviceException cannotRequestFilesMicroserviceException => 
                        new ExceptionResponse (new
                        {
                            code = cannotRequestFilesMicroserviceException.Code,
                            reason = cannotRequestFilesMicroserviceException.Message
                        },HttpStatusCode.InternalServerError),
                    CannotRequestIdentityMicroserviceException cannotRequestIdentityMicroserviceException => 
                        new ExceptionResponse (new
                        {
                            code = cannotRequestIdentityMicroserviceException.Code,
                            reason = cannotRequestIdentityMicroserviceException.Message
                        },HttpStatusCode.InternalServerError),
                    CannotRequestPetsMicroserviceException cannotRequestPetsMicroserviceException => 
                        new ExceptionResponse (new
                        {
                            code = cannotRequestPetsMicroserviceException.Code,
                            reason = cannotRequestPetsMicroserviceException.Message
                        },HttpStatusCode.InternalServerError),
                    InvalidShelterIdException invalidShelterIdException => 
                        new ExceptionResponse (new
                        {
                            code = invalidShelterIdException.Code,
                            reason = invalidShelterIdException.Message
                        },HttpStatusCode.BadRequest),
                    InvalidUserIdException invalidUserIdException => 
                        new ExceptionResponse (new
                        {
                            code = invalidUserIdException.Code,
                            reason = invalidUserIdException.Message
                        },HttpStatusCode.BadRequest),
                    PetDoesNotBelongToUserException petDoesNotBelongToUserException => 
                        new ExceptionResponse (new
                        {
                            code = petDoesNotBelongToUserException.Code,
                            reason = petDoesNotBelongToUserException.Message
                        },HttpStatusCode.Forbidden),
                    PetNotFoundException petNotFoundException => 
                        new ExceptionResponse (new
                        {
                            code = petNotFoundException.Code,
                            reason = petNotFoundException.Message
                        },HttpStatusCode.NotFound),
                    PetsNotFoundException petsNotFoundException => 
                        new ExceptionResponse (new
                        {
                            code = petsNotFoundException.Code,
                            reason = petsNotFoundException.Message
                        },HttpStatusCode.NotFound),
                    PhotoNotFoundException photoNotFoundException => 
                        new ExceptionResponse (new
                        {
                            code = photoNotFoundException.Code,
                            reason = photoNotFoundException.Message
                        },HttpStatusCode.NotFound),
                    UserDoesNotLikePetException userDoesNotLikePetException => 
                        new ExceptionResponse (new
                        {
                            code = userDoesNotLikePetException.Code,
                            reason = userDoesNotLikePetException.Message
                        },HttpStatusCode.BadRequest),
                    UserLikePetAlreadyException userLikePetAlreadyException => 
                        new ExceptionResponse (new
                        {
                            code = userLikePetAlreadyException.Code,
                            reason = userLikePetAlreadyException.Message
                        },HttpStatusCode.BadRequest),
                    UserNotOwnerOfShelterException userNotOwnerOfShelterException => 
                        new ExceptionResponse (new
                        {
                            code = userNotOwnerOfShelterException.Code,
                            reason = userNotOwnerOfShelterException.Message
                        },HttpStatusCode.Forbidden),
                    VisitNotFoundException visitNotFoundException => 
                        new ExceptionResponse (new
                        {
                            code = visitNotFoundException.Code,
                            reason = visitNotFoundException.Message
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