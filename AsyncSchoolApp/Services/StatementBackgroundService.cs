using AsyncBankApp.Dtos;
using System.Threading.Channels;

namespace AsyncBankApp.Services
{
    public class StatementBackgroundService : BackgroundService
    {
        private readonly Channel<TransactionExporterJob> _channel;
        private readonly IServiceProvider _serviceProvider;

        public StatementBackgroundService(Channel<TransactionExporterJob> channel, IServiceProvider serviceProvider)
        {
            _channel = channel;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach (var req in _channel.Reader.ReadAllAsync(stoppingToken))
                {
                    using var scope = _serviceProvider.CreateScope();
                    var transactionService = scope.ServiceProvider.GetRequiredService<TransactionService>();

                    try
                    {
                        var transactions = await transactionService.GetTransactionList(req.UserId, req.StartDate, req.EndDate);
                        await Task.Delay(1000);
                        var fileName = await transactionService.SaveExcelFileAsync(transactions);
                        // save file name to datebase 
                        // send notif to user
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing transaction: {ex.Message}");
                    }
                }
            }
        }
    }
}
