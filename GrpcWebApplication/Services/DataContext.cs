namespace GrpcWebApplication.Services
{
    using GrpcWebApplication.Models;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

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
    }
}
