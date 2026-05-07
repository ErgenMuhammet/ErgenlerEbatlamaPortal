using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.AccountingTransactionQuery.GetInvoices
{
    public class GetAllInvoicesQueryResponse
    {
        public bool IsSucces { get; set; }
        public string? Message { get; set; }

        public List<InvoiceDto>? NaturalGasInvoices { get; set; }
        public List<InvoiceDto>? WaterInvoices { get; set; }
        public List<InvoiceDto>? ElectricInvoices { get; set; }
        public List<InvoiceDto>? OtherInvoices { get; set; }

        public List <InvoiceDto>? PaidInvoice { get; set; }
        
    }
}
