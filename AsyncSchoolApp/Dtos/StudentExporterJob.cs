namespace AsyncSchoolApp.Dtos
{
    public class StudentExporterJob
    {
        public Guid Id { get; set; }
        public string Path { get; set; } = default!;
    }
}
