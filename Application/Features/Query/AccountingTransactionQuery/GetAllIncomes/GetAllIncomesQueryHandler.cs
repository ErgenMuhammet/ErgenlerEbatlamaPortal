using Application.DTOs;
using Application.Interface;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Entitiy;
using Application.Features.Query.AccountingTransactionQuery.GetAllOrder;

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
                    Incomes = null
                };
            }

            var Incomes = await _context.Incomes.Where(x => x.OwnerId == request.OwnerId).Select(x => new IncomesDto
            {
               Amount = x.Amount,
               Description = x.Description,
               IncomeDate = x.IncomeDate,

            }).AsNoTracking().ToListAsync(cancellationToken);

            if (Incomes.Count > 0)
            {
                return new GetAllIncomesQueryResponse
                {
                    IsSucces = true,
                    Message = "Gelir listesi başarıyla getirildi",
                    Incomes = Incomes
                };
            }
            else if(Incomes == null || Incomes.Count == 0)
            {
                return new GetAllIncomesQueryResponse
                {
                    IsSucces = true,
                    Incomes = null,
                    Message = "Görüntülenecek bir gelir bulunamadı"
                };
            }

            return new GetAllIncomesQueryResponse
            {
                IsSucces = false,
                Incomes = null,
                Message = "Gelirler listelenirken bir hata oluştu."
            };



        }
    }
}
