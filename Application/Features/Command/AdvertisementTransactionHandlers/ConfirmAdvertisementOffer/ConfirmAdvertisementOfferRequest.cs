using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.AdvertisementTransactionHandlers.ConfirmAdvertisementOffer
{
    public class ConfirmAdvertisementOfferRequest : IRequest<ConfirmAdvertisementOfferResponse>
    {
        [JsonIgnore]
        public string? AdvertisementId { get; set; }
        
        [JsonIgnore]
        public string? OwnerId { get; set; }
    }
}
