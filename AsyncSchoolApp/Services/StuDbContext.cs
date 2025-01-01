using AsyncSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncSchoolApp.Services
{
    public class StuDbContext : DbContext
    {
        public StuDbContext(DbContextOptions<StuDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
    }
}