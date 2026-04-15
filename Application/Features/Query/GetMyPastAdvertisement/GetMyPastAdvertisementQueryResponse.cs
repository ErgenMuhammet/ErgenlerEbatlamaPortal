using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetMyPastAdvertisement
{
    public class GetMyPastAdvertisementQueryResponse
    {
        public string Message { get; set; }
        public bool IsSucces { get; set; }
        public List<AdvertisementDto>? Advs { get; set; }
    }
}
