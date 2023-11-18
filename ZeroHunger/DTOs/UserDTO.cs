using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace ZeroHunger.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Name can only contain characters.")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }

        [Required]
        [PasswordValidation(ErrorMessage = "Password must be 8 characters long and contain at least 2 special characters.")]
        public string Password { get; set; }

        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^01[7-9][0-9]{8}$", ErrorMessage = "Invalid Contact Number")]
        public string ContactNumber { get; set; }
        [Required]
        public string Type { get; set; }
    }

    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string password = (string)value;

            if (string.IsNullOrEmpty(password) || password.Length != 8)
            {
                return new ValidationResult("Password must be 8 characters long.");
            }

            int specialCharacterCount = password.Count(c => !char.IsLetterOrDigit(c));
            if (specialCharacterCount < 2)
            {
                return new ValidationResult("Password must contain at least 2 special characters.");
            }

            return ValidationResult.Success;
        }
    }
}
