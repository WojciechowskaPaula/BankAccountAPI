using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankAccountAPI.Models
{
    public class User
    {
        [Required]
        [JsonIgnore]
        public int UserID { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First name cannot be longer than 30 characters and less than 2 characters")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First name cannot be longer than 30 characters and less than 2 characters")]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\s*\+?\s*([0-9][\s-]*){9,}$", ErrorMessage="Not valid Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Nationality name must contain minimum 3 characters")]
        public string Nationality { get; set; }
    }
}
