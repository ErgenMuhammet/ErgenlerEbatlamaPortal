using Domain.Entitiy.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ScrapsConfiguration : IEntityTypeConfiguration<Scraps>
{
    public void Configure(EntityTypeBuilder<Scraps> builder)
    {
        builder.ToTable("Scraps");
        builder.HasIndex(x => new { x.Color, x.Thickness, x.OwnerID  , x.Width , x.Height}).IsUnique();
        
        
        builder.Property(x => x.Color).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Width).IsRequired();
        builder.Property(x => x.Weight).IsRequired();
        builder.Property(x => x.Thickness).IsRequired();
        builder.Property(x => x.Height).IsRequired();
        builder.Property(x => x.Stock).HasDefaultValue(1);

        builder.HasOne(x => x.Owner)
               .WithMany(u => u.Scraps) 
               .HasForeignKey(x => x.OwnerID)
               .OnDelete(DeleteBehavior.Cascade); 



    }
}