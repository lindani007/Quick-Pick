using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace QuickPickDBApi.Models.dbContext_folder
{
    public class QuickPickDbContext : DbContext
    {
        public QuickPickDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Aisle> Aisles { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<BoughtItem> BoughtItems { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Login> Employees { get; set; }
    }
}
