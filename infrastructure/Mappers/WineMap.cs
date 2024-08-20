using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappers
{
    internal class WineMap : IEntityTypeConfiguration<Wine>
    {
        public void Configure(EntityTypeBuilder<Wine> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(w => w.Harvest)
                .IsRequired();

            builder.Property(w => w.Region)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(w => w.Winery)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(w => w.Grape)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
