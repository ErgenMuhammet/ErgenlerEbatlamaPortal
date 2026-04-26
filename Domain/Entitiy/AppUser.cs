using Domain.Entitiy.Material;
using Domain.GlobalEnum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy
{
    public class AppUser : IdentityUser
    {
        
        public string? FullName { get; set; }
        public string? PhotoUrl { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; } 
        public DateTime CreatedDate { get; set; } = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
        public bool IsDeleted { get; set; } = false;
        public string? City { get; set; }
        public bool IsUpdated { get; set; }
        

        public BaseJobs? Jobs{ get; set; }
       
        public ICollection<Mdf>? Mdf { get; set; }
        public ICollection<BackPanel>? BackPanel { get; set; } 
        public ICollection<Glue>? Glue { get; set; }
        public ICollection<PvcBand>? PvcBand { get; set; }
        public ICollection<Scraps>? Scraps { get; set; }
        public ICollection<Invoice>? Invoice { get; set; } = new List<Invoice>();
        public ICollection<Expense>? Expense { get; set; }
        public ICollection<Income>?  Income { get; set; }
        public Guid? SituationId { get; set; } 
        public List<ProfitLossSituation>? Situation { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Advertisements>? Advertisements{ get; set; }

        public Category UserCategory { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
