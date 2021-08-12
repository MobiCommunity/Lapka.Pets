using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete
{
    public class ValueCreated : IDomainEvent
    {
        public Value Value { get; }

        public ValueCreated(Value value)
        {
            Value = value;
        }
    }
}