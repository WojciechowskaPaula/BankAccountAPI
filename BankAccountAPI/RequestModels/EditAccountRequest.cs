using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BankAccountAPI.RequestModels
{
    public class EditAccountRequest
    {
        [Required]
        [DisplayName("Account ID")]
        public int AccountId { get; set; }

        [Required]
        [DisplayName("Bank name")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Bank name cannot be longer than 30 characters and less than 2 characters")]
        public string BankName { get; set; }

        [Required]
        [DisplayName("Account number")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Account number is not valid")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The Account number must contain only numbers")]
        public string AccountNumber { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "IBAN must contain minimum 8 characters")]
        public string IBAN { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "The Currency must contain maximium 30 characters")]
        public string Currency { get; set; }
    }
}
