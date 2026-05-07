using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Query.GetTheBiddersAdvertisements
{
    public class GetTheBiddersAdvertisementRequest : IRequest<GetTheBiddersAdvertisementResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }

        [JsonIgnore]
        public string? BiddersId { get; set; }
    }
}
