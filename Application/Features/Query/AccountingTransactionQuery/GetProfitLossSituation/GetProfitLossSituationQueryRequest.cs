using MediatR;

namespace Application.Features.Query.AccountingTransactionQuery.GetProfitLossSituation
{
    public class GetProfitLossSituationQueryRequest : IRequest<GetProfitLossSituationQueryResponse>
    {
        public string OwnerId { get; set; }
    }
}
