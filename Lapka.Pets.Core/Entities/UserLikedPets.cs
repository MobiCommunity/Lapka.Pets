using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Events.Concrete.Pets.Like;

namespace Lapka.Pets.Core.Entities
{
    public class UserLikedPets : AggregateRoot
    {
        private ISet<Guid> _likedPets = new HashSet<Guid>();
        public Guid UserId { get; }

        public IEnumerable<Guid> LikedPets
        {
            get => _likedPets;
            private set => _likedPets = new HashSet<Guid>(value);
        }

        public UserLikedPets(Guid userId, IEnumerable<Guid> likedPets)
        {
            Id = new AggregateId(userId);
            UserId = userId;
            LikedPets = likedPets;
        }

        public static UserLikedPets Create(Guid userId, IEnumerable<Guid> likedPets)
        {
            UserLikedPets userLikedPets = new UserLikedPets(userId, likedPets);
            userLikedPets.AddEvent(new CreatedUserLikedPets(userLikedPets));
            return userLikedPets;
        }

        public void RemoveLike(Guid petId)
        {
            _likedPets.Remove(petId);
            AddEvent(new RemovedPetLike(this));
        }
        
        public void AddLike(Guid petId)
        {
            _likedPets.Add(petId);
            AddEvent(new AddedPetLike(this));
        }
    }
}