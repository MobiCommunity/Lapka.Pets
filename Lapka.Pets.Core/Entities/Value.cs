using System;
using Lapka.Pets.Core.Events.Concrete;

namespace Lapka.Pets.Core.Entities
{
    public class Value : AggregateRoot
    {
        public string Name {get; }
        public string Description { get;}

        public Value(Guid id,string name, string description)
        {
            Id = new AggregateId(id);
            Name = name;
            Description = description;
        }

        public static Value Create(Guid id,string name, string description)
        {
            Value user = new Value(id, name, description);
            user.AddEvent(new ValueCreated(user));
            return user;
        }
        
    }
}