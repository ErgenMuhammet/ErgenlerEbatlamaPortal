using Application.DTOs;
using Application.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Entitiy;

namespace Application.Features.Query.AccountingTransactionQuery.GetAllIncomes
{    
    public class GetAllIncomesQueryHandler : IRequestHandler<GetAllIncomesQueryRequest, GetAllIncomesQueryResponse>
    {
        private readonly IAppContext _context ;
        private readonly UserManager<AppUser> _userManager;

        public GetAllIncomesQueryHandler(IAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<GetAllIncomesQueryResponse> Handle(GetAllIncomesQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.OwnerId);

            if (user == null)
            {
                return new GetAllIncomesQueryResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı",
                };
            }

            try
            {
                var CreditCardIncome = await _context.Incomes
                    .Where(x => x.OwnerId == request.OwnerId && x.IncomeType == "CreditCard")
                    .Select(x => new IncomesDto
                    {
                        Id = x.Id.ToString(),
                        Amount = x.Amount,
                        Description = x.Description,
                        IncomeDate = x.IncomeDate,
                        IncomeType = x.IncomeType,
                    }).AsNoTracking().ToListAsync(cancellationToken);

                var CashIncome = await _context.Incomes
                    .Where(x => x.OwnerId == request.OwnerId && x.IncomeType == "Cash")
                    .Select(x => new IncomesDto
                    {
                        Id = x.Id.ToString(),
                        Amount = x.Amount,
                        Description = x.Description,
                        IncomeDate = x.IncomeDate,
                        IncomeType = x.IncomeType,
                    }).AsNoTracking().ToListAsync(cancellationToken);

                var OtherIncome = await _context.Incomes
                    .Where(x => x.OwnerId == request.OwnerId && x.IncomeType == "Other")
                    .Select(x => new IncomesDto
                    {
                        Id = x.Id.ToString(),
                        Amount = x.Amount,
                        Description = x.Description,
                        IncomeDate = x.IncomeDate,
                        IncomeType = x.IncomeType,
                    }).AsNoTracking().ToListAsync(cancellationToken);

                return new GetAllIncomesQueryResponse
                {
                    OtherIncomes = OtherIncome,
                    CashIncomes = CashIncome,
                    CreditCardIncomes = CreditCardIncome,
                    IsSucces = true,
                    Message = "Gelir listesi başarıyla getirildi"
                };
            }
            catch (Exception ex)
            {
                return new GetAllIncomesQueryResponse
                {
                    IsSucces = false,
                    Message = $"Gelirler listelenirken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                };
            }
        }
    }
}
