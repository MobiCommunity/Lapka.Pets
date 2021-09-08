using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Users
{
    public class UserPetDeleted : IDomainEvent
    {
        public UserPet Pet { get; }

        public UserPetDeleted(UserPet pet)
        {
            Pet = pet;
        }
    }
}