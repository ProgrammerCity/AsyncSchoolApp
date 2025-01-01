using AsyncSchoolApp.Models;
using OfficeOpenXml;

namespace AsyncSchoolApp.Services
{
    public class StudentService
    {
        private readonly StudentRepository _studentRepository;
        public StudentService(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task InitializeDatabase()
        {
            await _studentRepository.InitializeDatabase();
        }

        public async Task<List<Student>> GetStudentList(int? age)
        {
            return await _studentRepository.GetAllStudents(age);
        }

        public async Task<string> SaveExcelFileAsync(List<Student> students,Guid id)
        {
            var fileName = $"Students_{id}.xlsx";
            var filePath = Path.Combine("Exports", fileName);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Students");

                // Add header row
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "First Name";
                worksheet.Cells[1, 3].Value = "Last Name";
                worksheet.Cells[1, 4].Value = "Age";
                worksheet.Cells[1, 5].Value = "Code";

                // Add data rows
                for (int i = 0; i < students.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = students[i].Id;
                    worksheet.Cells[i + 2, 2].Value = students[i].FirstName;
                    worksheet.Cells[i + 2, 3].Value = students[i].LastName;
                    worksheet.Cells[i + 2, 4].Value = students[i].Age;
                    worksheet.Cells[i + 2, 5].Value = students[i].StudentCode;
                }

                worksheet.Cells.AutoFitColumns();

                await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await package.SaveAsAsync(fileStream);
            }

            // Generate the download URL
            return fileName;

        }
    }
}

