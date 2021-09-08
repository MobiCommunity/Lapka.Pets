using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Users
{
    public class UserPetUpdated : IDomainEvent
    {
        public UserPet Pet { get; }

        public UserPetUpdated(UserPet pet)
        {
            Pet = pet;
        }
    }
}