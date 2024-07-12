using Common.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
#nullable disable
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<LGA> LGAs { get; set; }
    }
}
