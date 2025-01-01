namespace AsyncBankApp.Dtos
{
    public class TransactionExporterJob
    {
        public Guid Id { get; set; }
        public string Path { get; set; } = default!;
    }
}
