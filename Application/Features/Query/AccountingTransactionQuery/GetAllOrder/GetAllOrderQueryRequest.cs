using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Query.AccountingTransactionQuery.GetAllOrder
{
    public class GetAllIncomesQueryRequest : IRequest<GetAllIncomesQueryResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }
    }
}
