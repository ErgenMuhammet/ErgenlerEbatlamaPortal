using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetAdvertisement
{
    public class GetAdvertisementQueryResponse
    {
        public string Message { get; set; }
        public bool IsSucces {  get; set; }
        public AdvertisementDto Advs { get; set; }
    }
}
