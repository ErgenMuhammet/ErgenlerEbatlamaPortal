using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.AccountingTransactionCommands.UpdateExpense
{
    public class UpdateExpenseCommandResponse
    {
        public bool IsSucces { get; set; }
        public string? Message { get; set; }

        
    }
}
