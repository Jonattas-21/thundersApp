using Domain.Entities;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Databases
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<TaskForce> TaskForces { get; set; }
        public DbSet<Origin> Origins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskForceMap());
            modelBuilder.ApplyConfiguration(new OriginMap());

            base.OnModelCreating(modelBuilder);
        }

    }
}
