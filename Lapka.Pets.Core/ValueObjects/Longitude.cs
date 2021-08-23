using Lapka.Pets.Core.Exceptions.Location;

namespace Lapka.Pets.Core.ValueObjects
{
    public class Longitude : LocationParam
    {
        public Longitude(string value) : base(value)
        {
            
        }
        
        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                throw new InvalidLongitudeValueException(Value);
            }

            bool isDouble = double.TryParse(Value, out double doubleLongitude);

            if (!isDouble)
            {
                throw new LongitudeIncorrectDataTypeException(Value);
            }

            if (doubleLongitude <= MinLongitudeValue)
            {
                throw new LongitudeTooLowException(Value);
            }

            if (doubleLongitude >= MaxLongitudeValue)
            {
                throw new LongitudeTooBigException(Value);
            }
        }
        
        private const int MaxLongitudeValue = 180;
        private const int MinLongitudeValue = -180;
    }
}