using Domain.Entitiy.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BackPanelConfiguration : IEntityTypeConfiguration<BackPanel>
{
    public void Configure(EntityTypeBuilder<BackPanel> builder)
    {
        builder.ToTable("BackPanels"); // Çoğul isim kullanımı standarttır
        builder.HasKey(x => x.Id);

        // Böylece: "Aynı kullanıcı, aynı marka-renk-kalınlıkta ikinci bir kayıt açamaz."
        builder.HasIndex(x => new { x.Brand, x.Color, x.Thickness, x.OwnerID }).IsUnique();

        builder.Property(x => x.Brand).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Color).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Thickness).IsRequired();
        builder.Property(x => x.Stock).HasDefaultValue(0);

        // Kullanıcı ile ilişkiyi burada tanımlıyoruz
        builder.HasOne(x => x.Owner)
               .WithMany(u => u.BackPanel) // AppUser içindeki ICollection ismi
               .HasForeignKey(x => x.OwnerID)
               .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse stoğu da gider
    }
}