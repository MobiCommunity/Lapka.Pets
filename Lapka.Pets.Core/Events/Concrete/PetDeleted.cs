using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete
{
    public class PetDeleted<T> : IDomainEvent where T : Pet 
    {
        public T Pet { get; }

        public PetDeleted(T pet)
        {
            Pet = pet;
        }
    }
}