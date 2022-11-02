using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PetTracker.Shared.Validators
{

    /// <summary>
    /// Decimal precision validator data annotation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class DecimalPrecisionAttribute : ValidationAttribute
    {
        private readonly uint _decimalPrecision;

        public DecimalPrecisionAttribute(uint decimalPrecision)
        {
            _decimalPrecision = decimalPrecision;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null || value is decimal d && HasPrecision(d, _decimalPrecision))
                return ValidationResult.Success;
            else
                return new ValidationResult($"Broj može sadržavati maksimalno {_decimalPrecision} decimale.");
        }

        private static bool HasPrecision(decimal value, uint precision)
        {
            string valueStr = value.ToString(CultureInfo.InvariantCulture);
            int indexOfDot = valueStr.IndexOf('.');
            if (indexOfDot == -1)
            {
                return true;
            }

            return valueStr.Length - indexOfDot - 1 <= precision;
        }
    }
}