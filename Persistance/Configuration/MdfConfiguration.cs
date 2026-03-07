using Domain.Entitiy.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MdfConfiguration : IEntityTypeConfiguration<Mdf>
{
    public void Configure(EntityTypeBuilder<Mdf> builder)
    {

        builder.ToTable("Mdfs");

        builder.HasIndex(x => new { x.Brand, x.Color, x.Thickness,x.OwnerID }).IsUnique();
        
        builder.Property(x => x.Brand).HasColumnName("Brand").HasMaxLength(100);
        builder.Property(x => x.Thickness).HasColumnName("Thickness").HasMaxLength(200);
        builder.Property(x => x.Color).HasColumnName("Color").HasMaxLength(100);
        builder.Property(x => x.Stock).HasColumnName("Stock");

        builder.HasOne(x => x.Owner).
            WithMany(u => u.Mdf).
            HasForeignKey(x => x.OwnerID).
            OnDelete(DeleteBehavior.Cascade);   

    }
}
