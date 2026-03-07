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
    public class ProfitLossSituationConfiguration : IEntityTypeConfiguration<ProfitLossSituation>
    {
        public void Configure(EntityTypeBuilder<ProfitLossSituation> builder) 
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("ProfitLossSituation");

            builder.Property(x => x.OwnerId).IsRequired();
            builder.Property(x => x.TotalProfit).HasDefaultValue(0);
            builder.Property(x => x.TotalLoss).HasDefaultValue(0);
            builder.Property(x => x.LastSituation).IsRequired(false);
            builder.Property(x => x.Date).IsRequired();

            builder.HasOne(x => x.Owner).
                WithMany().
                HasForeignKey(x => x.OwnerId).
                OnDelete(DeleteBehavior.NoAction);


        }
    }
}
