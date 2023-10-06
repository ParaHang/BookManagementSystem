using BookManagementSystem.Common.Models;
using Microsoft.EntityFrameworkCore;
namespace BookManagementSystemOrder.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _options = options;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("InMemoryDb");
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<BookOrder> BookOrders { get; set; }
    }
}
