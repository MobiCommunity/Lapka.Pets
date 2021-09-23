using Convey.WebApi.Exceptions;
using System;
using System.Net;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Core.Exceptions;
using Lapka.Pets.Core.Exceptions.Abstract;
using Lapka.Pets.Core.Exceptions.Location;
using Lapka.Pets.Core.Exceptions.Pet;

namespace Lapka.Pets.Infrastructure.Exceptions
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                DomainException ex => ex switch
                {
                    InvalidAddressNameException invalidAddressNameException => new ExceptionResponse(new
                    {
                        code = invalidAddressNameException.Code,
                        reason = invalidAddressNameException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidCityValueException invalidCityValueException => new ExceptionResponse(new
                    {
                        code = invalidCityValueException.Code,
                        reason = invalidCityValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidStreetValueException invalidStreetValueException => new ExceptionResponse(new
                    {
                        code = invalidStreetValueException.Code,
                        reason = invalidStreetValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidLatitudeValueException invalidLatitudeValueException => new ExceptionResponse(new
                    {
                        code = invalidLatitudeValueException.Code,
                        reason = invalidLatitudeValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidLongitudeValueException invalidLongitudeValueException => new ExceptionResponse(new
                    {
                        code = invalidLongitudeValueException.Code,
                        reason = invalidLongitudeValueException.Message
                    }, HttpStatusCode.BadRequest),
                    LatitudeIncorrectDataTypeException latitudeIncorrectDataTypeException => new ExceptionResponse(new
                    {
                        code = latitudeIncorrectDataTypeException.Code,
                        reason = latitudeIncorrectDataTypeException.Message
                    }, HttpStatusCode.BadRequest),
                    LatitudeTooBigException latitudeTooBigException => new ExceptionResponse(new
                    {
                        code = latitudeTooBigException.Code,
                        reason = latitudeTooBigException.Message
                    }, HttpStatusCode.BadRequest),
                    LatitudeTooLowException latitudeTooLowException => new ExceptionResponse(new
                    {
                        code = latitudeTooLowException.Code,
                        reason = latitudeTooLowException.Message
                    }, HttpStatusCode.BadRequest),
                    LongitudeIncorrectDataTypeException longitudeIncorrectDataTypeException => new ExceptionResponse(new
                    {
                        code = longitudeIncorrectDataTypeException.Code,
                        reason = longitudeIncorrectDataTypeException.Message
                    }, HttpStatusCode.BadRequest),
                    LongitudeTooBigException longitudeTooBigException => new ExceptionResponse(new
                    {
                        code = longitudeTooBigException.Code,
                        reason = longitudeTooBigException.Message
                    }, HttpStatusCode.BadRequest),
                    LongitudeTooLowException longitudeTooLowException => new ExceptionResponse(new
                    {
                        code = longitudeTooLowException.Code,
                        reason = longitudeTooLowException.Message
                    }, HttpStatusCode.BadRequest),
                    DescriptionTooShortException descriptionTooShortException => new ExceptionResponse(new
                    {
                        code = descriptionTooShortException.Code,
                        reason = descriptionTooShortException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidBirtDayValueException invalidBirtDayValueException => new ExceptionResponse(new
                    {
                        code = invalidBirtDayValueException.Code,
                        reason = invalidBirtDayValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidBirthDayValueException invalidBirthDayValueException => new ExceptionResponse(new
                    {
                        code = invalidBirthDayValueException.Code,
                        reason = invalidBirthDayValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidColorValueException invalidColorValueException => new ExceptionResponse(new
                    {
                        code = invalidColorValueException.Code,
                        reason = invalidColorValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidAggregateIdException invalidAggregateIdException => new ExceptionResponse(new
                    {
                        code = invalidAggregateIdException.Code,
                        reason = invalidAggregateIdException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidDescriptionValueException invalidDescriptionValueException => new ExceptionResponse(new
                    {
                        code = invalidDescriptionValueException.Code,
                        reason = invalidDescriptionValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidLostDateValueException invalidLostDateValueException => new ExceptionResponse(new
                    {
                        code = invalidLostDateValueException.Code,
                        reason = invalidLostDateValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidOwnerNameValueException invalidOwnerNameValueException => new ExceptionResponse(new
                    {
                        code = invalidOwnerNameValueException.Code,
                        reason = invalidOwnerNameValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidPetNameException invalidPetNameException => new ExceptionResponse(new
                    {
                        code = invalidPetNameException.Code,
                        reason = invalidPetNameException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidPhoneNumberException invalidPhoneNumberException => new ExceptionResponse(new
                    {
                        code = invalidPhoneNumberException.Code,
                        reason = invalidPhoneNumberException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidRaceValueException invalidRaceValueException => new ExceptionResponse(new
                    {
                        code = invalidRaceValueException.Code,
                        reason = invalidRaceValueException.Message
                    }, HttpStatusCode.BadRequest),
                    WeightBelowMinimumValueException weightBelowMinimumValueException => new ExceptionResponse(new
                    {
                        code = weightBelowMinimumValueException.Code,
                        reason = weightBelowMinimumValueException.Message
                    }, HttpStatusCode.BadRequest),
                    InvalidBucketNameException invalidBucketNameException => new ExceptionResponse(new
                    {
                        code = invalidBucketNameException.Code,
                        reason = invalidBucketNameException.Message
                    }, HttpStatusCode.BadRequest),
                    _ => new ExceptionResponse(new {code = ex.Code, reason = ex.Message},
                        HttpStatusCode.BadRequest),
                },

                AppException ex => ex switch
                {
                    CannotConvertShelterPetCountIntoIntException cannotConvertShelterPetCountIntoIntException => 
                        new ExceptionResponse (new
                        {
                            code = cannotConvertShelterPetCountIntoIntException.Code,
                            reason = cannotConvertShelterPetCountIntoIntException.Message
                        },HttpStatusCode.InternalServerError),
                    InvalidPetIdException invalidPetIdException => 
                        new ExceptionResponse (new
                        {
                            code = invalidPetIdException.Code,
                            reason = invalidPetIdException.Message
                        },HttpStatusCode.BadRequest),
                    ShelterDoesNotExistsException shelterDoesNotExistsException => 
                        new ExceptionResponse (new
                        {
                            code = shelterDoesNotExistsException.Code,
                            reason = shelterDoesNotExistsException.Message
                        },HttpStatusCode.NotFound),
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