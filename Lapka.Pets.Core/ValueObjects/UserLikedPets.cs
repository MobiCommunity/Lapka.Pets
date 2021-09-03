using System;
using System.Collections.Generic;

namespace Lapka.Pets.Core.ValueObjects
{
    public class UserLikedPets
    {
        public Guid UserId { get; }
        public List<Guid> LikedPets { get; }

        public UserLikedPets(Guid userId, List<Guid> likedPets)
        {
            UserId = userId;
            LikedPets = likedPets;
        }
    }
}