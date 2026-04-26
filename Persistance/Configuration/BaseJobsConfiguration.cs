using Domain.Entitiy;
using Domain.GlobalEnum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class BaseJobsConfiguration : IEntityTypeConfiguration<BaseJobs>
    {
        public void Configure(EntityTypeBuilder<BaseJobs> builder)
        {

            builder.HasKey(e => e.Id);

            builder.HasNoDiscriminator();

            builder.HasOne(x => x.User).
                WithOne(y => y.Jobs).
                HasForeignKey<BaseJobs>(x => x.UserId).
                OnDelete(DeleteBehavior.Cascade);

            builder.Property(j => j.WorkShopName).HasMaxLength(200);
            builder.Property(j => j.MastersName).HasMaxLength(100);
            builder.Property(j => j.PhoneNumber).HasMaxLength(20);

        }
        
    }
}
