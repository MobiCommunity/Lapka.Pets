using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.UserPets
{
    public class UpdateUserPetPhoto : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public File Photo { get; }

        public UpdateUserPetPhoto(Guid id, Guid userId, File photo)
        {
            Id = id;
            UserId = userId;
            Photo = photo;
        }
    }
}