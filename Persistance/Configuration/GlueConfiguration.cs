using Domain.Entitiy.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GlueConfiguration : IEntityTypeConfiguration<Glue>
{
    public void Configure(EntityTypeBuilder<Glue> builder)
    {
        builder.ToTable("Glues");

        builder.HasKey(x => x.Id);

        // Ağırlık ve Markaya göre stok benzersizliği

        builder.HasIndex(x => new { x.Brand, x.Weight ,x.OwnerID}).IsUnique();

        builder.Property(x => x.Brand).HasColumnName("Brand");
        builder.Property(x => x.Weight).HasColumnName("Weight");
        builder.Property(x => x.Stock).HasColumnName("Stock");

        builder.HasOne(x => x.Owner).
            WithMany(u => u.Glue).
            HasForeignKey(x => x.OwnerID).
            OnDelete(DeleteBehavior.Cascade);
    }
}