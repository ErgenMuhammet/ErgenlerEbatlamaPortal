using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Query.GetAdvertisement
{
    public class GetAdvertisementQueryRequest : IRequest <GetAdvertisementQueryResponse>
    {
        [JsonIgnore]
        public string AdvertismentId { get; set; }
    }
}
