using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Mappers
{
    internal class OriginMap : IEntityTypeConfiguration<Origin>
    {
        public void Configure(EntityTypeBuilder<Origin> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(g => g.CreatedAt)
                .IsRequired();

            builder.Property(g => g.UpdatedAt)
                .IsRequired();

            builder.Property(g => g.Ativo)
                .IsRequired();
        }
    }
}
