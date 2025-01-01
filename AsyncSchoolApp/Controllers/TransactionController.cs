using AsyncBankApp.Dtos;
using AsyncBankApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AsyncBankApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly TransactionService _TransactionService;
        public TransactionController(ILogger<TransactionController> logger, TransactionService TransactionService)
        {
            _TransactionService = TransactionService;
            _logger = logger;
        }

        [HttpGet(Name = "GetAllTransaction")]
        public async Task<IActionResult> Get(int userId, long? startDate, long? endDate)
        {
            var Transaction = await _TransactionService.GetTransactionList(userId, startDate, endDate);
            var fileName = await _TransactionService.SaveExcelFileAsync(Transaction);
            var fileUrl = $"{Request.Scheme}://{Request.Host}/Exports/{fileName}";
            return Ok(new TransactionListDto() { Success = true, TransactionCount = Transaction.Count, Link = fileUrl });
        }

        [HttpPost("initialize")]
        public async Task<IActionResult> InitializeDatabase()
        {
            await _TransactionService.InitializeDatabase();
            return Ok("Database initialized with 10000 fake Transactions.");
        }
    }
}
