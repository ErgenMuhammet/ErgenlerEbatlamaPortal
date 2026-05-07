using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetTheBiddersAdvertisements
{
    public class GetTheBiddersAdvertisementResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<AdvertisementDto>? BiddersAdvertisements{ get; set; }
    }
}
