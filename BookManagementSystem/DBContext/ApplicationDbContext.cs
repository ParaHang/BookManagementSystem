using BookManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.DBContext
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

        public DbSet<Book> Books { get; set; }
    }
}
