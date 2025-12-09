using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Million.Properties.Domain.Entities;

namespace Million.Properties.Infrastructure.Persistence.ModelBuilders;

public class PropertyImageModelBuilder : IEntityTypeConfiguration<PropertyImage>
{
    public void Configure(EntityTypeBuilder<PropertyImage> builder)
    {
        builder.ToTable("PropertyImages");
        builder.HasKey(x => x.IdPropertyImage);
        builder.Property(x => x.File)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Enabled)
            .IsRequired();

        builder.Property(x => x.IdProperty)
            .IsRequired();

        builder.HasOne(pi => pi.Property)             
            .WithMany(p => p.Images)                   
            .HasForeignKey(pi => pi.IdProperty)     
            .OnDelete(DeleteBehavior.Cascade);
    }
}
