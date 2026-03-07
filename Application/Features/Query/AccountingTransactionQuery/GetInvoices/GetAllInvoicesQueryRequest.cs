using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Query.AccountingTransactionQuery.GetInvoices
{
    public class GetAllInvoicesQueryRequest : IRequest <GetAllInvoicesQueryResponse> 
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }
    }
}
