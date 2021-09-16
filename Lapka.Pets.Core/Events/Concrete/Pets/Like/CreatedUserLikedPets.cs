using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Like
{
    public class CreatedUserLikedPets : IDomainEvent
    {
        public UserLikedPets UserLikedPets { get; }

        public CreatedUserLikedPets(UserLikedPets userLikedPets)
        {
            UserLikedPets = userLikedPets;
        }
    }
}