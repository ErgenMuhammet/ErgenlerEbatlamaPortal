using Domain.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy
{
    public class Advertisements 
    {
        public Guid AdvertisementId { get; set; } = Guid.NewGuid();
        public DateTime AdvertisementDate { get; set;} = DateTime.Now.Date;
        public string Title { get; set; } 
        public string AdvertisementAddress { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public AppUser? Owner { get; set; }
        public string OwnerId { get; set; }
        public string ImgUrl { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? Bidder { get; set; }
        public Category TargetCategory { get; set; } = Category.Assembler | Category.Carpenter;
        public Category WhoCanShare { get; set; } = Category.Carpenter;
    }
}
