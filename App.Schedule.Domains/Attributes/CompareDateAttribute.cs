using System;
using System.ComponentModel.DataAnnotations;

namespace App.Schedule.Domains.Attributes
{
    public sealed class CompareDateAttribute : ValidationAttribute
    {
        private string _startDatePropertyName;

        public CompareDateAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (value != null)
            {
                if ((DateTime)value >= (DateTime)propertyValue)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Please enter valid time.");
                }
            }
            else
            {
                return new ValidationResult("Please enter valid time.");
            }
        }
    }
}
