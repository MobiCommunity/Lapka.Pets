using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete.Pets.Users
{
    public class UserPetCreated : IDomainEvent
    {
        public UserPet Pet { get; }
        
        public UserPetCreated(UserPet pet)
        {
            Pet = pet;
        }

    }
}