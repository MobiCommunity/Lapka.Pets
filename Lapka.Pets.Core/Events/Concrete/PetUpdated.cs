using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete
{
    public class PetUpdated<T> : IDomainEvent where T : AggregatePet
    {
        public T Pet { get; }

        public PetUpdated(T pet)
        {
            Pet = pet;
        }
    }
}