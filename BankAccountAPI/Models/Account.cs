namespace BankAccountAPI.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN { get; set; }
        public string Currency { get; set; }
        public User User { get; set; }
    }
}
