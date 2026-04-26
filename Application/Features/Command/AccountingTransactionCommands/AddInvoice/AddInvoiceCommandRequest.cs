using Domain.Entitiy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.AccountingTransaction.AddInvoice
{
    public class AddInvoiceCommandRequest : IRequest<AddInvoiceCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }
        public string? InvoiceName { get; set; }
        public string? InvoiceNo { get; set; }
        public float Cost { get; set; }
        public DateTime LastPaymentDate { get; set; }
        
        
    }
}
