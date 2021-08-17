using Lapka.Pets.Core.Exceptions.Location;

namespace Lapka.Pets.Core.ValueObjects
{
    public class Latitude : LocationParam
    {
        public Latitude(string value) : base(value)
        {
        }
        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                throw new InvalidLatitudeValueException(Value);
            }

            bool isDouble = double.TryParse(Value, out double doubleLatitude);

            if (!isDouble)
            {
                throw new LatitudeIncorrectDataTypeException(Value);
            }

            if (doubleLatitude <= MinLatitudeValue)
            {
                throw new LatitudeTooLowException(Value);
            }

            if (doubleLatitude >= MaxLatitudeValue)
            {
                throw new LatitudeTooBigException(Value);
            }
        }
        
        private const int MaxLatitudeValue = 90;
        private const int MinLatitudeValue = -90;
    }
}