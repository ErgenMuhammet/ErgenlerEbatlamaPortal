using Domain.Entitiy.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PvcBandConfiguration : IEntityTypeConfiguration<PvcBand>
{
    public void Configure(EntityTypeBuilder<PvcBand> builder)
    {
        builder.ToTable("PvcBands");

        builder.HasIndex(x => new { x.Brand, x.Color, x.Thickness , x.OwnerID}).IsUnique();

        builder.Property(x => x.Brand).HasColumnName("Brand");
        builder.Property(x => x.Color).HasMaxLength(50);
        builder.Property(x => x.Thickness).HasColumnName("Thickness");
        builder.Property(x => x.Stock).HasColumnName("Stock");
        
        builder.HasOne(x =>  x.Owner).
            WithMany(u => u.PvcBand).
            HasForeignKey(x => x.OwnerID).
            OnDelete(DeleteBehavior.Cascade);

    }
}