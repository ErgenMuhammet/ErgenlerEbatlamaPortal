using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.AdvertisementTransactionHandlers.OfferToAdvertisement
{
    public class OfferToAdvertisementCommandRequest : IRequest<OfferToAdvertisementCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        public string AdvertisementId { get; set; }
    }
}
