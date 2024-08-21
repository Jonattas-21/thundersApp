using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Mappers
{
    internal class TaskForceMap : IEntityTypeConfiguration<TaskForce>
    {
        public void Configure(EntityTypeBuilder<TaskForce> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Priority)
                .IsRequired();

            builder.Property(w => w.Assignee)
                .IsRequired()
                .HasMaxLength(100); ;

            builder.Property(w => w.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(w => w.CreatedAt)
                .IsRequired();

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}
