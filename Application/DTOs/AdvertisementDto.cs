using Domain.Entitiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AdvertisementDto
    {
        public string Id { get; set; }
        public DateTime AdvertisementDate { get; set; } 
        public string Title { get; set; }
        public string AdvertisementAddress { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string OwnerId { get; set; }
        public string ImgUrl { get; set; }
        public bool? IsActive { get; set; }
    }
}
