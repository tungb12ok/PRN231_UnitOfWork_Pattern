using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Validate;

public class ValidStatusAttribute : ValidationAttribute
{
    private readonly string[] _validStatuses = { "Pending", "Approved", "Rejected" };

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || _validStatuses.Contains(value.ToString()))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult($"Status must be one of the following values: {string.Join(", ", _validStatuses)}");
    }
}