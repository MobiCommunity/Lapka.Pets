using System.Text.RegularExpressions;
using Lapka.Pets.Core.Exceptions.Pet;

namespace Lapka.Pets.Core.ValueObjects
{
    public class PhoneNumber
    {
        public string Value { get; }

        public PhoneNumber(string phoneNumber)
        {
            Value = phoneNumber;
            
            Validate();
        }

        private void Validate()
        {
            if (!PhoneNumberRegex.IsMatch(Value))
            {
                throw new InvalidPhoneNumberException(Value);
            }
        }
        
        private static readonly Regex PhoneNumberRegex =
            new Regex(@"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
    }
}