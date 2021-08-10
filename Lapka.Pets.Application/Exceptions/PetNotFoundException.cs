using System;
using System.Security.AccessControl;

namespace Lapka.Pets.Application.Exceptions
{
    public class PetNotFoundException : AppException
    {
        public Guid Id { get; }
        public override string Code => "pet_not_found";

        public PetNotFoundException(Guid id) : base($"Pet not exists in database: {id}")
        {
            Id = id;
        }

    }
}