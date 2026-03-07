using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.AccountingTransaction.PayTheInvoice
{
    public class PayTheInvoiceCommandResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
