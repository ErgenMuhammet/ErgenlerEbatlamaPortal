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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderName).IsRequired();
            builder.Property(x => x.MetreOfPvcBand).IsRequired(false);
            builder.Property(x => x.CountOfBackPanel).IsRequired(false);
            builder.Property(x => x.CountOfMdf).IsRequired(false);
            builder.Property(x => x.CostOfOrder).IsRequired(false);

            builder.HasOne(x => x.Owner).
                      WithMany( u => u.Orders).
                           HasForeignKey(x => x.OwnerId).
                                OnDelete(DeleteBehavior.Cascade);

        }
    }
}
