using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.AdvertisementTransactionHandlers.OfferToAdvertisement
{
    public class OfferToAdvertisementCommandResponse
    {
        public bool IsSucces { get; set; }
        public string? Message { get; set; }
        
        public string BidderId { get; set; } 
    }
}
