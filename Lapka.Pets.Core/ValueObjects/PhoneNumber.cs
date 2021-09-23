using System.Text.RegularExpressions;
using Lapka.Pets.Core.Exceptions.Pet;

namespace Lapka.Pets.Core.ValueObjects
{
    public class PhoneNumber
    {
        public string Value { get; }

        public PhoneNumber(string phoneNumber)
        {
            ValidatePhoneNumber(phoneNumber);
            
            Value = phoneNumber;
        }

        private void ValidatePhoneNumber(string phoneNumber)
        {
            if (!PhoneNumberRegex.IsMatch(phoneNumber))
            {
                throw new InvalidPhoneNumberException(phoneNumber);
            }
        }
        
        private static readonly Regex PhoneNumberRegex =
            new Regex(@"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
    }
}