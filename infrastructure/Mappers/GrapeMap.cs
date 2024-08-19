using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Mappers
{
    internal class GrapeMap : IEntityTypeConfiguration<Grape>
    {
        public void Configure(EntityTypeBuilder<Grape> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.Origin)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(g => g.Description)
                .IsRequired()
                .HasMaxLength(500);

        }
    }
}
