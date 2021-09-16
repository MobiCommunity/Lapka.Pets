using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.LostPets
{
    public class UpdateLostPetPhoto : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public PhotoFile Photo { get; }

        public UpdateLostPetPhoto(Guid id, Guid userId, PhotoFile photo)
        {
            Id = id;
            UserId = userId;
            Photo = photo;
        }
    }
}