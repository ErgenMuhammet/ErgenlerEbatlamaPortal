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
    public class IncomeConfiguration : IEntityTypeConfiguration<Income>
    {
        public void Configure(EntityTypeBuilder<Income> builder)
        {
            builder.ToTable("Income");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.IncomeDate).IsRequired(false);
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            
            builder.HasOne(x => x.Owner).
                WithMany(u => u.Income).
                HasForeignKey(x => x.OwnerId).
                OnDelete(DeleteBehavior.Cascade);
 

        }
    }
}
