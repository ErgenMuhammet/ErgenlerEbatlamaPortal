using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Query.GetMaterialQuery.GetMaterialStock
{
    public class GetMaterialStockQueryRequest : IRequest <GetMaterialStockQueryResponse<object>>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }
        public string? MaterialType { get; set; }
        
    }
}
