using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappers
{
    internal class AnalysisMap
    {
        public void Configure(EntityTypeBuilder<Analysis> builder)
        {
            builder.HasKey(w => w.Wine.Id);

            builder.Property(w => w.Acidity)
                .IsRequired();

            builder.Property(w => w.Sweet)
                .IsRequired();

            builder.Property(w => w.Alcohol)
                .IsRequired();

            builder.Property(w => w.Body)
                .IsRequired();

            builder.Property(w => w.AdditionNotes)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(w => w.Wine)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
