using System;
using Convey.CQRS.Commands;

namespace Lapka.Pets.Application.Commands
{
    public class LikePet : ICommand
    {
        public Guid PetId { get; }
        public Guid UserId { get; }

        public LikePet(Guid petId, Guid userId)
        {
            PetId = petId;
            UserId = userId;
        }
    }
}