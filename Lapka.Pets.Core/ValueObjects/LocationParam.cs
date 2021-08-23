using System;

namespace Lapka.Pets.Core.ValueObjects
{
    public abstract class LocationParam
    {
        public string Value { get; }

        protected LocationParam(string value)
        {
            Value = value;
        }

        public abstract void Validate();
        public virtual double AsDouble() => double.Parse(Value);
    }
}