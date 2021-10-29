using GrpcWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace GrpcWebApplication.Services
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase("inMemoryDatabase");
            //optionsBuilder.UseSqlite("Data source=data.db");
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Test;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().HasKey(e => e.Id);
            modelBuilder.Entity<Message>().Property(e => e.Id).ValueGeneratedOnAdd();
        }

        public DbSet<Message> Messages { get; set; }
    }
}
