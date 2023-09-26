using BookManagementSystem.Common;
using BookManagementSystem.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.DBContext
{
    public class ApplicationDbContext: IdentityDbContext<Users, Roles, string>
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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Users>().ToTable("AspNetUsers");
            builder.Entity<Roles>().ToTable("AspNetRoles");
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
