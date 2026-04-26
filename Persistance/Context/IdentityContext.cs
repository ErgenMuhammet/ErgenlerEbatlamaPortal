using Application.Interface;
using Domain.Entitiy;
using Domain.Entitiy.Material;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class IdentityContext : IdentityDbContext<AppUser, IdentityRole, string> , IAppContext
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
        

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        { 
            
        }

        // IdentityContext.cs içinde olmalı:
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Mutlaka en üstte kalmalı

            // Klasördeki TÜM konfigürasyonları otomatik yükle
            builder.ApplyConfigurationsFromAssembly(typeof(IdentityContext).Assembly);
                  
        }

        public async Task AddAsync(object entity, CancellationToken cancellationToken = default)
        {
            await base.AddAsync(entity, cancellationToken);
        }   
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

       
    }
}
