using Domain.Entitiy.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MdfConfiguration : IEntityTypeConfiguration<Mdf>
{
    public void Configure(EntityTypeBuilder<Mdf> builder)
    {

        builder.ToTable("Mdfs");

        builder.HasIndex(x => new { x.Brand, x.Color, x.Thickness,x.OwnerID }).IsUnique();

        builder.Property(x => x.Brand).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Color).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Thickness).IsRequired();
        builder.Property(x => x.Weight).IsRequired(); 
        builder.Property(x => x.Stock).HasDefaultValue(0);

        builder.HasOne(x => x.Owner).
            WithMany(u => u.Mdf).
            HasForeignKey(x => x.OwnerID).
            OnDelete(DeleteBehavior.Cascade);   

    }
}
