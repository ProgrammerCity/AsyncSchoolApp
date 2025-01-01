using AsyncBankApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncBankApp.Services
{
    public class TrcDbContext : DbContext
    {
        public TrcDbContext(DbContextOptions<TrcDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
    }
}