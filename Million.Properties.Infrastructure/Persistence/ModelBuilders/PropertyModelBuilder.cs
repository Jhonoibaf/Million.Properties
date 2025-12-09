using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Million.Properties.Domain.Entities;

namespace Million.Properties.Infrastructure.Persistence.ModelBuilders;

public class PropertyModelBuilder : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("Properties");

        builder.HasKey(x => x.IdProperty);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CodeInternal)
            .HasMaxLength(40);  

        builder.Property(x => x.Year)
            .IsRequired();

        builder.HasOne(p => p.Owner)
            .WithMany(o => o.Properties)
            .HasForeignKey(p => p.IdOwner)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Images)
            .WithOne(i => i.Property)
            .HasForeignKey(i => i.IdProperty)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Traces)
            .WithOne(t => t.Property)
            .HasForeignKey(t => t.IdProperty)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
