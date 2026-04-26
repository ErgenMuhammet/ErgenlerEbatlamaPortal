using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Query.AccountingTransactionQuery.GetProfitLossSituation
{
    public class GetProfitLossSituationQueryHandler : IRequestHandler<GetProfitLossSituationQueryRequest, GetProfitLossSituationQueryResponse>
    {
        private readonly IAppContext _context;

        public GetProfitLossSituationQueryHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<GetProfitLossSituationQueryResponse> Handle(GetProfitLossSituationQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _context.ProfitLossSituation
                .FirstOrDefaultAsync(p => p.OwnerId == request.OwnerId, cancellationToken);

            if (data == null)
            {
                return new GetProfitLossSituationQueryResponse
                {
                    IsSuccess = false,
                    Message = "Durum bulunamadı.",
                    Data = null
                };
            }

            return new GetProfitLossSituationQueryResponse
            {
                IsSuccess = true,
                Message = "Başarılı.",
                Data = data
            };
        }
    }
}
