using Domain.Entitiy;
using Domain.Entitiy.Material;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAppContext
    {
       public DbSet<AppUser> AppUsers { get; set; }
       public DbSet<BaseJobs> Jobs { get; set; }
       

       public DbSet<Mdf> Mdf { get; set; }
       public DbSet<Glue> Glue { get; set; }
       public DbSet<BackPanel> BackPanel { get; set; }
       public DbSet<Scraps> Scraps { get; set; }
       public DbSet<PvcBand> PvcBand { get; set; }
       public DbSet<Invoice> Invoice { get; set; }
       public DbSet<Income> Incomes { get; set; }
       public DbSet<Expense> Expense { get; set; }
       public DbSet<ProfitLossSituation> ProfitLossSituation { get; set; }
       public DbSet<Order> Orders { get; set; }
       public DbSet<Advertisements> Advertisements { get; set; }
       public DbSet<ChatMessage> Messages { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        public Task AddAsync(object entity, CancellationToken cancellationToken);    

    }
}
