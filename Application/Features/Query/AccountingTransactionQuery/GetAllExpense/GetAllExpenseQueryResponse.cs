using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.AccountingTransactionQuery.GetAllExpense
{
    public class GetAllExpenseQueryResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<ExpenseDto> CreditCardExpenses { get; set; }
        public List<ExpenseDto> CashExpenses { get; set; }
        public List<ExpenseDto> OtherExpenses { get; set; }

    }
}
