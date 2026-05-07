using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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

            var currentMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            var UserAccounting = await _context.ProfitLossSituation
                .FirstOrDefaultAsync(x => x.OwnerId == request.OwnerId &&
                                        x.Date.Value.Year == currentMonth.Year &&
                                              x.Date.Value.Month == currentMonth.Month, cancellationToken);

            if (UserAccounting == null)
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
                Data = UserAccounting
            };
        }
    }
}
