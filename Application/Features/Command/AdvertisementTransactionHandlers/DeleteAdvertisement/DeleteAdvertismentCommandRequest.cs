using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.AdvertisementTransactionHandlers.DeleteAdvertisement
{
    public class DeleteAdvertismentCommandRequest : IRequest <DeleteAdvertisementCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        [JsonIgnore]
        public string? AdvertisementId{ get; set; }
    }
}
