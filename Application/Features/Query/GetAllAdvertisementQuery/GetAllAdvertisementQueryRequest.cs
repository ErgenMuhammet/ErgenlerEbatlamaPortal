using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Query.GetAllAdvertisementQuery
{
    public class GetAllAdvertisementQueryRequest : IRequest<GetAllAdvertisementQueryResponse>
    {
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
