﻿using Domain.Entities;
using infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Databases
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Wine> Wines { get; set; }
        public DbSet<Grape> Grapes { get; set; }
        public DbSet<Analysis> Analyses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WineMap());
            modelBuilder.ApplyConfiguration(new GrapeMap());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("YourConnectionStringHere");
        }
    }
}
