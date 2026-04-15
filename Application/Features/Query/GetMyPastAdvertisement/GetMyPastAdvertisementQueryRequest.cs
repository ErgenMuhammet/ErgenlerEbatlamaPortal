using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Query.GetMyPastAdvertisement
{
    public class GetMyPastAdvertisementQueryRequest : IRequest<GetMyPastAdvertisementQueryResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }
    }
}
