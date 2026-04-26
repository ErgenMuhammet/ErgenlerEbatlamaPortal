using Domain.Entitiy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisements>
    {
        public void Configure(EntityTypeBuilder<Advertisements> builder)
        {
            builder.ToTable("Advertisements");

            builder.HasIndex(x => new { x.AdvertisementAddress, x.OwnerId, x.Title }).IsUnique(); 

            builder.HasKey(x => x.AdvertisementId);

            builder.Property(a => a.Title).IsRequired();

            builder.Property(b => b.AdvertisementAddress).IsRequired().HasMaxLength(250);

            builder.Property(c => c.ImgUrl).IsRequired();

            builder.Property(d => d.Latitude).
                HasColumnType("decimal(10 , 8)");

            builder.Property(d => d.Longitude).
                HasColumnType("decimal(11 , 8)");

            builder.HasOne(y => y.Owner).
                WithMany(u => u.Advertisements).
                HasForeignKey(x => x.OwnerId).
                OnDelete(DeleteBehavior.Cascade);
        }
    }
}
