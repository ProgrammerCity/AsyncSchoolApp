using AsyncBankApp.Dtos;
using System.Threading.Channels;

namespace AsyncBankApp.Services
{
    public class StatementBackgroundService(Channel<TransactionExporterJob> channel, TransactionService transactionService) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach (var req in channel.Reader.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        var Transactions = await transactionService.GetTransactionList(req.UserId, req.StartDate, req.EndDate);
                        var fileName = await transactionService.SaveExcelFileAsync(Transactions);
                        

                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
            }
        }
    }
}
