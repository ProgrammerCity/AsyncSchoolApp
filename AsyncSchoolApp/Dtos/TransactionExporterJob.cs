namespace AsyncBankApp.Dtos
{
    public class TransactionExporterJob
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
    }
}
