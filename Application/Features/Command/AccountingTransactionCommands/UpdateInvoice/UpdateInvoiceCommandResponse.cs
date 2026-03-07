using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.AccountingTransactionCommands.UpdateInvoice
{
    public class UpdateInvoiceCommandResponse
    {
        public bool IsSucces { get; set; }
        public string? Message { get; set; }
    }
}
