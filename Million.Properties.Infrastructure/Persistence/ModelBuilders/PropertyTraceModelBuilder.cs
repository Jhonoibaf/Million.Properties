using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Million.Properties.Domain.Entities;

namespace Million.Properties.Infrastructure.Persistence.ModelBuilders;

public class PropertyTraceConfiguration : IEntityTypeConfiguration<PropertyTrace>
{
    public void Configure(EntityTypeBuilder<PropertyTrace> builder)
    {
        builder.ToTable("PropertyTrace");

        builder.HasKey(x => x.IdPropertyTrace);

        builder.Property(x => x.DateSale)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Value)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Tax)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne(pt => pt.Property)
            .WithMany(p => p.Traces)
            .HasForeignKey(pt => pt.IdProperty)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
