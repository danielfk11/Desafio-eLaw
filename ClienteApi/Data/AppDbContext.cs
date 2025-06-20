using ClienteApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClienteApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().OwnsOne(c => c.Endereco);
            modelBuilder.Entity<Cliente>().HasIndex(c => c.Email).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
