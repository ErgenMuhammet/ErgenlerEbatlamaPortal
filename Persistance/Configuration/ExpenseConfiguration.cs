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
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expense");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.ExpenseDate);
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasOne(x => x.Owner).
                WithMany(u => u.Expense).
                HasForeignKey(x => x.OwnerId).
                OnDelete(DeleteBehavior.Cascade);

           


        }
    }
}
