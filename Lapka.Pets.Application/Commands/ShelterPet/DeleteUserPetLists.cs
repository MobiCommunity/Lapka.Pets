using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands
{
    public class DeleteUserPetLists : ICommand
    {
        public Guid UserId { get; }

        public DeleteUserPetLists(Guid userId)
        {
            UserId = userId;
        }
    }
}