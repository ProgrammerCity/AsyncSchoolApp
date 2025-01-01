namespace AsyncBankApp.Dtos
{
    public class TransactionListDto
    {
        public int TransactionCount { get; set; }
        public bool Success { get; set; }
        public string Link { get; set; }
    }
}
