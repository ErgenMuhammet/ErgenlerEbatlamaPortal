using Application.DTOs;
using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Query.AccountingTransactionQuery.GetInvoices
{
    public class GetAllInvoicesQueryHandler : IRequestHandler<GetAllInvoicesQueryRequest,GetAllInvoicesQueryResponse>
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GetAllInvoicesQueryHandler(UserManager<AppUser> userManager, IAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<GetAllInvoicesQueryResponse> Handle(GetAllInvoicesQueryRequest request, CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);

            if (Owner == null)
            {
                return new GetAllInvoicesQueryResponse
                {
                    IsSucces = false,
                    Invoices = null,
                    Message = "Kullanıcı bilgisine ulaşılamadı.",

                };
            }

            List<InvoiceDto> Invoices = await _context.Invoice.Where(x => x.OwnerId == Owner.Id).Select(m => new InvoiceDto
            {
                Id = m.Id.ToString(),
                LastPaymentDate = m.LastPaymentDate,
                Name = m.InvoiceName,
                InvoicesNo = m.InvoiceNo,
                Price = m.Cost,

            }).AsNoTracking().ToListAsync();

            if (Invoices.Count == 0)
            {
                return new GetAllInvoicesQueryResponse
                {
                    IsSucces = true,
                    Invoices = Invoices,
                    Message = "Listelenecek fatura bulunamadı"
                };
            }

            return new GetAllInvoicesQueryResponse
            {
                IsSucces = true,
                Invoices = Invoices,
                Message = "Faturalar başarı ile listelendi"
            };
        }
    }
}