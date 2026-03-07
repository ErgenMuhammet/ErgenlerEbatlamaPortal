using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.AccountingTransactionCommands.UpdateInvoice
{
    public class UpdateInvoiceCommandRequest : IRequest<UpdateInvoiceCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }
        [JsonIgnore]
        public string? InvoiceId { get; set; }
        public string? InvoiceName { get; set; }
        public string? InvoiceNo { get; set; }
        public float Cost { get; set; }
        public DateTime? LastPaymentDate { get; set; }
    }
}
