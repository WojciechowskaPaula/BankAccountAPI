using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankAccountAPI.Models
{
    public class Account
    {
        [Required]
        [DisplayName("Account ID")]
        public int AccountId { get; set; }

        [Required]
        [DisplayName("Bank name")]
        [StringLength(30, MinimumLength =3, ErrorMessage = "Bank name cannot be longer than 30 characters and less than 2 characters")]
        public string BankName { get; set; }

        [Required]
        [DisplayName("Account number")]
        [StringLength(20, MinimumLength =7, ErrorMessage ="Account number is not valid")]
        [RegularExpression("^[^-][0 - 9]$", ErrorMessage ="The Account number must contain only numbers")]
        public string AccountNumber { get; set; }

        [Required]
        [MinLength(8)]
        public string IBAN { get; set; }

        [Required]
        [MaxLength(30)]
        public string Currency { get; set; }
        public User User { get; set; }
    }
}
