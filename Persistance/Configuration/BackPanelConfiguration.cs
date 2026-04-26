using Domain.Entitiy.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BackPanelConfiguration : IEntityTypeConfiguration<BackPanel>
{
    public void Configure(EntityTypeBuilder<BackPanel> builder)
    {
        builder.ToTable("BackPanels"); 
        builder.HasKey(x => x.Id);

       
        builder.HasIndex(x => new { x.Brand, x.Color, x.Thickness, x.OwnerID }).IsUnique();

        builder.Property(x => x.Brand).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Color).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Thickness).IsRequired();
        builder.Property(x => x.Stock).HasDefaultValue(0);

        builder.HasOne(x => x.Owner)
               .WithMany(u => u.BackPanel) 
               .HasForeignKey(x => x.OwnerID)
               .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse stoğu da gider
    }
}