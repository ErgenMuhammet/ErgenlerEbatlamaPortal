using Application.DTOs;
using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.AccountingTransactionQuery.GetAllExpense
{
    public class GetAllExpenseQueryHandler : IRequestHandler<GetAllExpenseQueryRequest,GetAllExpenseQueryResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppContext _context;

        public GetAllExpenseQueryHandler(UserManager<AppUser> userManager, IAppContext context)
        {   
            _userManager = userManager;
            _context = context;
        }

        public async Task<GetAllExpenseQueryResponse> Handle(GetAllExpenseQueryRequest request, CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);

            if (Owner == null)
            {
                return new GetAllExpenseQueryResponse
                {
                    Expenses = null,
                    IsSuccess = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı.Daha sonra tekrar deneyiniz."
                };
            }

            var ExpenseList = await _context.
                Expense.
                Where(x => x.OwnerId == request.OwnerId).
                Select(m => new ExpenseDto
                {
                    Id = m.Id.ToString(),
                    Amount = m.Amount,
                    Description = m.Description,
                    ExpenseDate = m.ExpenseDate,
                }).AsNoTracking().ToListAsync(cancellationToken);

            if (ExpenseList.Count == 0)
            {
                return new GetAllExpenseQueryResponse
                {
                    Expenses = null,
                    IsSuccess = false,
                    Message = "Görüntülenecek harcama bulunmamaktadır."
                };
            }

            if (ExpenseList.Count > 0)
            {
                return new GetAllExpenseQueryResponse
                {
                    Expenses = ExpenseList,
                    IsSuccess = true,
                    Message = "Harcamalar Listelendi."
                };
            }

            return new GetAllExpenseQueryResponse
            {
                Expenses = null,
                IsSuccess = false,
                Message = "Harcamalar görüntülenirken bir hata ile karşılaşıldı."
            };
        }

    }
}
