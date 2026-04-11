using Domain.Entitiy;
using Domain.Entitiy.Jobs;
using Domain.GlobalEnum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class BaseJobsConfiguration : IEntityTypeConfiguration<BaseJobs>
    {
        public void Configure(EntityTypeBuilder<BaseJobs> builder)
        {
            
            builder.HasKey(e => e.Id);

            builder.HasDiscriminator<Category>("BaseJobs").
                HasValue(Category.Carpenter).
                                HasValue(Category.Carpenter).
                                                HasValue(Category.Carpenter);
            builder.HasOne(x => x.User).
                WithOne(y => y.Jobs).
                HasForeignKey<BaseJobs>(x => x.UserId).
                IsRequired(false).
                OnDelete(DeleteBehavior.Cascade);

            

        }
        
    }
}
