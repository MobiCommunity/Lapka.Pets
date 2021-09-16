using System;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Like
{
    public class RemovedPetLike : IDomainEvent
    {
        public UserLikedPets LikedPets { get; }

        public RemovedPetLike(UserLikedPets likedPets)
        {
            LikedPets = likedPets;
        }
    }
}