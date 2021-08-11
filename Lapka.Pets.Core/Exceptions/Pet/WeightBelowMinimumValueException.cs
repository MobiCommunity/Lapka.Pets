using Lapka.Pets.Core.Exceptions.Abstract;

namespace Lapka.Pets.Core.Exceptions.Pet
{
    public class WeightBelowMinimumValueException : DomainException
    {
        public double Weight { get; }
        public WeightBelowMinimumValueException(double weight) : base($"Weight value is zero or below zero: {weight}")
        {
            Weight = weight;
        }

        public override string Code => "weight_below_minimum_value";
    }
}