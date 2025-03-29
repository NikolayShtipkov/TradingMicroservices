using Microsoft.EntityFrameworkCore;
using PortfolioApi.Models;

namespace PortfolioApi.Data
{
    public class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options) { }

        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<PortfolioHolding> PortfolioHoldings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PortfolioHolding>()
                .HasOne(p => p.Portfolio)
                .WithMany(p => p.Holdings)
                .HasForeignKey(p => p.PortfolioId);
        }
    }
}
