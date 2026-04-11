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
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices"); ;

            builder.HasIndex(x => new { x.InvoiceNo, x.InvoiceName , x.OwnerId , x.Cost }).IsUnique();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.InvoiceNo).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Cost).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.LastPaymentDate).IsRequired();
            builder.Property(x => x.InvoiceName).IsRequired();
            builder.Property(x => x.BeenPaid).HasDefaultValue(false);


            builder.
                HasOne(a => a.Owner).
                WithMany(k => k.Invoice).
                HasForeignKey(a => a.OwnerId).
                OnDelete(DeleteBehavior.Cascade);

        }
    }
}

