using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands.ShelterPets
{
    public class CreateUserLikeList  : ICommand
    {
        public Guid UserId { get; }

        public CreateUserLikeList(Guid userId)
        {
            UserId = userId;
        }
    }
}