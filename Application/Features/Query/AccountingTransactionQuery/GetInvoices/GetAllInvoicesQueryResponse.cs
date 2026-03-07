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
        public List<InvoiceDto> Invoices { get; set; }

    }
}
