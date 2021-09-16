using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Events.Concrete.Pets.Like;

namespace Lapka.Pets.Core.Entities
{
    public class UserLikedPets : AggregateRoot
    {
        public Guid UserId { get; }
        public List<Guid> LikedPets { get; }

        public UserLikedPets(Guid userId, List<Guid> likedPets)
        {
            Id = new AggregateId(userId);
            UserId = userId;
            LikedPets = likedPets;
        }

        public static UserLikedPets Create(Guid userId, List<Guid> likedPets)
        {
            UserLikedPets userLikedPets = new UserLikedPets(userId, likedPets);
            userLikedPets.AddEvent(new CreatedUserLikedPets(userLikedPets));
            return userLikedPets;
        }

        public void RemoveLike(Guid petId)
        {
            LikedPets.Remove(petId);
            AddEvent(new RemovedPetLike(this));
        }
        
        public void AddLike(Guid petId)
        {
            LikedPets.Add(petId);
            AddEvent(new AddedPetLike(this));
        }
    }
}