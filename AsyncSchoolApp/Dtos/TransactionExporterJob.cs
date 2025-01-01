namespace AsyncBankApp.Dtos
{
    public class TransactionExporterJob(
        int userId,
        long? startDate,
        long? endDate)
    {
        public int UserId { get; set; } = userId;
        public long? StartDate { get; set; } = startDate;
        public long? EndDate { get; set; } = endDate;
    }
}
