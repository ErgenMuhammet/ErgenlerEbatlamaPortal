using Domain.Entitiy.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PvcBandConfiguration : IEntityTypeConfiguration<PvcBand>
{
    public void Configure(EntityTypeBuilder<PvcBand> builder)
    {
        builder.ToTable("PvcBands");

        builder.HasIndex(x => new { x.Brand, x.Color, x.Thickness , x.OwnerID }).IsUnique();

        builder.Property(x => x.Brand).HasColumnName("Brand").IsRequired().HasMaxLength(50); 
        builder.Property(x => x.Color).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Thickness).HasColumnName("Thickness").IsRequired();
        builder.Property(x => x.Stock).HasColumnName("Stock").HasDefaultValue(0);

        builder.HasOne(x =>  x.Owner).
            WithMany(u => u.PvcBand).
            HasForeignKey(x => x.OwnerID).
            OnDelete(DeleteBehavior.Cascade);

    }
}