using System.Threading.Channels;
using AsyncBankApp.Dtos;
using AsyncBankApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AsyncBankApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController(Channel<TransactionExporterJob> channel, TransactionService transactionService) : ControllerBase
    {

        [HttpGet(Name = "GetAllTransaction")]
        public async Task<IActionResult> Get(int userId, long? startDate, long? endDate)
        {

            await channel.Writer.WriteAsync(new TransactionExporterJob(userId, startDate, endDate));
            //var Transaction = await transactionService.GetTransactionList(userId, startDate, endDate);
            //var fileName = await transactionService.SaveExcelFileAsync(Transaction);
            //var fileUrl = $"{Request.Scheme}://{Request.Host}/Exports/{fileName}";
            return Accepted(new { Success = true });
        }

        [HttpPost("initialize")]
        public async Task<IActionResult> InitializeDatabase()
        {
            await transactionService.InitializeDatabase();
            return Ok("Database initialized with 10000 fake Transactions.");
        }
    }
}
