using AsyncBankApp.Models;
using OfficeOpenXml;

namespace AsyncBankApp.Services
{
    public class TransactionService
    {
        private readonly TransactionRepository _TransactionRepository;
        public TransactionService(TransactionRepository TransactionRepository)
        {
            _TransactionRepository = TransactionRepository;
        }

        public async Task InitializeDatabase()
        {
            await _TransactionRepository.InitializeDatabase();
        }

        public async Task<List<Transaction>> GetTransactionList(int userId, long? startDate, long? endDate)
        {
            return await _TransactionRepository.GetAllTransactions(userId,startDate, endDate);
        }

        public async Task<string> SaveExcelFileAsync(List<Transaction> transactions)
        {
            var fileName = $"Trc_{Guid.NewGuid()}.xlsx";
            var filePath = Path.Combine("Exports", fileName);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Transactions");

                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "First Name";
                worksheet.Cells[1, 3].Value = "Last Name";
                worksheet.Cells[1, 4].Value = "UserId";
                worksheet.Cells[1, 5].Value = "SubmitDate";
                worksheet.Cells[1, 6].Value = "Amount";

                for (int i = 0; i < transactions.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = transactions[i].Id;
                    worksheet.Cells[i + 2, 2].Value = transactions[i].FirstName;
                    worksheet.Cells[i + 2, 3].Value = transactions[i].LastName;
                    worksheet.Cells[i + 2, 4].Value = transactions[i].UserId;
                    worksheet.Cells[i + 2, 5].Value = transactions[i].SubmitDate.ToShortDateString();
                    worksheet.Cells[i + 2, 6].Value = transactions[i].Amount.ToString("N0");
                }

                worksheet.Cells.AutoFitColumns();

                await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await package.SaveAsAsync(fileStream);
            }

            return fileName;
        }
    }
}

