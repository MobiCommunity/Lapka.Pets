using System;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Like
{
    public class AddedPetLike : IDomainEvent
    {
        public UserLikedPets LikedPets { get; }

        public AddedPetLike(UserLikedPets likedPets)
        {
            LikedPets = likedPets;
        }
    }
}