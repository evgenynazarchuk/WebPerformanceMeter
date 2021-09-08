﻿namespace RestWebApplication.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RestWebApplication.Models;

    public class DataAccess : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source=data.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(product => product.Id);
            modelBuilder.Entity<FileStorage>().HasKey(storage => storage.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
