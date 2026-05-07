using Domain.Entitiy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(t => t.Id);
            builder.ToTable("Notification");

            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.OwnerId).IsRequired();
            
            builder.HasOne(x => x.Owner).
                WithMany().
                HasForeignKey(x => x.OwnerId).
                OnDelete(DeleteBehavior.Cascade);

        }
    }
}
