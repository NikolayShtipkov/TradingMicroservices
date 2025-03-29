using Microsoft.EntityFrameworkCore;
using PriceApi.Models;

namespace PriceApi.Data
{
    public class PriceDbContext : DbContext
    {
        public PriceDbContext(DbContextOptions<PriceDbContext> options) : base(options) { }

        public DbSet<Price> Prices { get; set; }
    }
}
