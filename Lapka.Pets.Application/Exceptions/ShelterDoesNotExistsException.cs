using System;

namespace Lapka.Pets.Application.Exceptions
{
    public class ShelterDoesNotExistsException : AppException
    {
        public Guid ShelterId { get; }
        public ShelterDoesNotExistsException(Guid shelterId) : base($"Shelter does not exists: {shelterId}")
        {
            ShelterId = shelterId;
        }

        public override string Code => "shelter_does_not_exists";
    }
}