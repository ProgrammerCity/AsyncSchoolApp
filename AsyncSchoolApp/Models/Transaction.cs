namespace AsyncBankApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime SubmitDate { get; set; } 
        public int Amount { get; set; }
    }
}
