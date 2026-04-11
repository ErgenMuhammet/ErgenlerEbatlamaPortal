using Domain.Entitiy;
using Domain.Entitiy.Jobs;
using Domain.Entitiy.Material;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");
            builder.HasKey(x => x.Id);
            

            builder.HasOne(x => x.Jobs).
                WithOne(x => x.User).
                HasForeignKey<BaseJobs>(x => x.UserId).
                OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(x => x.Invoice).
                WithOne(x => x.Owner).
                HasForeignKey(x => x.OwnerId).
                OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Income).
               WithOne(x => x.Owner).
               HasForeignKey(x => x.OwnerId).
               OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Orders).
           WithOne(x => x.Owner).
           HasForeignKey(x => x.OwnerId).
           OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Expense).
          WithOne(x => x.Owner).
          HasForeignKey(x => x.OwnerId).
          OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(x => x.Mdf).
               WithOne(x => x.Owner).
               HasForeignKey(x => x.OwnerID).
               OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.BackPanel).
             WithOne(x => x.Owner).
             HasForeignKey(x => x.OwnerID).
             OnDelete(DeleteBehavior.Cascade);

             builder.HasMany(x => x.PvcBand).
            WithOne(x => x.Owner).
            HasForeignKey(x => x.OwnerID).
            OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Scraps).
           WithOne(x => x.Owner).
           HasForeignKey(x => x.OwnerID).
           OnDelete(DeleteBehavior.Cascade);         

            builder.HasMany(x => x.Glue).
           WithOne(x => x.Owner).
           HasForeignKey(x => x.OwnerID).
           OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
