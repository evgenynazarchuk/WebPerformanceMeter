namespace GrpcWebApplication.Services
{
    using GrpcWebApplication.Models;
    using Microsoft.EntityFrameworkCore;

    public class WritableDataAccess : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase("inMemoryDatabase");
            optionsBuilder.UseSqlite("Data source=data.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().HasKey(e => e.Id);
        }
    }
}
