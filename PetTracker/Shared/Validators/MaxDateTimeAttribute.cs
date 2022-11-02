using System.ComponentModel.DataAnnotations;

namespace PetTracker.Shared.Validators
{
    /// <summary>
    /// Maximum datetime value validator data annotation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class MaxDateTimeAttribute : ValidationAttribute
    {
        private readonly DateTime _maxDateTime;

        public MaxDateTimeAttribute()
        {
            _maxDateTime = DateTime.UtcNow.AddMinutes(-2);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var dateTime = (DateTime)value;

            return DateTime.Compare(dateTime.ToUniversalTime(), _maxDateTime) > 0
                ? new ValidationResult($"Datum ne može biti noviji od: {_maxDateTime.ToLocalTime()}")
                : ValidationResult.Success;
        }
    }
}
