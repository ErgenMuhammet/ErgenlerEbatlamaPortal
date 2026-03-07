using Application.DTOs;
using Domain.Entitiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.AccountingTransactionQuery.GetAllIncomes
{
    public class GetAllIncomesQueryResponse
    {
        public bool IsSucces { get; set; }
        public string? Message { get; set; }
        public IList<IncomesDto> Incomes { get; set; }
    }
}
