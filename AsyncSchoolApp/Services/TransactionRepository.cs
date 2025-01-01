using AsyncBankApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;

namespace AsyncBankApp.Services
{
    public class TransactionRepository
    {
        private readonly TrcDbContext _context;

        public TransactionRepository(TrcDbContext context)
        {
            _context = context;
        }
        public async Task InitializeDatabase()
        {
            _context.Database.EnsureCreated();

            if (!await _context.Transactions.AnyAsync())
            {
                Random rnd = new();
                DateTime start = new(1990, 1, 1);
                int range = (DateTime.Today - start).Days;
                int id = 1;
                for (int i = 1; i <= 100; i++)
                {
                    for (int c = 1; c <= 100; c++)
                    {
                        await _context.Transactions.AddAsync(new Transaction
                        {
                            Id = id,
                            UserId = i,
                            FirstName = $"FirstName{i}",
                            LastName = $"LastName{i}",
                            SubmitDate = start.AddDays(rnd.Next(range)),
                            Amount = rnd.Next(1_000_000, 5_000_000)
                        });
                        id++;
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Transaction>> GetAllTransactions(long? startDate, long? endDate)
        {
            //await Task.Delay(10_000);
            return await _context.Transactions
                .Where(x => startDate == null || x.SubmitDate == DateTimeOffset.FromUnixTimeMilliseconds(startDate.Value).DateTime)
                .Where(x => endDate == null || x.SubmitDate == DateTimeOffset.FromUnixTimeMilliseconds(endDate.Value).DateTime)
                .ToListAsync();
        }
    }
}
