using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Query.GetMaterialQuery.GetScraps
{
    public class GetAllScrapsQueryRequest : IRequest<GetAllScrapsQueryResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }
    }
}
