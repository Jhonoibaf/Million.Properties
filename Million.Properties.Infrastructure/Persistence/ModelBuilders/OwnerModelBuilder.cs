using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Million.Properties.Domain.Entities;

namespace Million.Properties.Infrastructure.Persistence.ModelBuilders;

public class OwnerModelBuilder : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.ToTable("Owners");

        builder.HasKey(x => x.IdOwner);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Address)
            .HasMaxLength(250);

        builder.Property(x => x.Photo)
            .HasMaxLength(300);

        builder.Property(x => x.Birthday)
            .IsRequired();

        builder.HasMany(o => o.Properties)
            .WithOne(p => p.Owner)
            .HasForeignKey(p => p.IdOwner)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
