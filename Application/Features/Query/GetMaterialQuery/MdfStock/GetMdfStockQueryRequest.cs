using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Query.GetMaterialQuery.MdfStock
{
    public class GetMdfStockQueryRequest : IRequest<GetMdfStockQueryResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }
    }
}
