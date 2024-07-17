using Microsoft.EntityFrameworkCore;
using AuthAPI.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AuthAPI.Models;

namespace AuthAPI.Data
{
    public class MyDbContext : IdentityUserContext<User>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        // public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}
