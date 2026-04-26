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

namespace Application.Features.Query.AccountingTransactionQuery.GetPayedInvoice
{
    public class GetPayedInvoiceQueryHandler : IRequestHandler<GetPayedInvoiceQueryRequest, GetPayedInvoiceQueryResponse>
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GetPayedInvoiceQueryHandler(UserManager<AppUser> userManager, IAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }
          
        public async Task<GetPayedInvoiceQueryResponse> Handle(GetPayedInvoiceQueryRequest request, CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);

            if (Owner == null)
            {
                return new GetPayedInvoiceQueryResponse
                {
                    IsSuccess = false,
                    Invoices = null,
                    Message = "Kullanıcı bilgisine ulaşılamadı.",
                };
            }

            List<InvoiceDto> Invoices = await _context.Invoice
                .Where(x => x.OwnerId == Owner.Id && x.BeenPaid == true)
                .Select(m => new InvoiceDto
                {
                    Id = m.Id.ToString(),
                    LastPaymentDate = m.LastPaymentDate,
                    Name = m.InvoiceName,
                    InvoicesNo = m.InvoiceNo,
                    Price = m.Cost
                }).AsNoTracking().ToListAsync();

            if (Invoices.Count == 0)
            {
                return new GetPayedInvoiceQueryResponse
                {
                    IsSuccess = true,
                    Invoices = Invoices,
                    Message = "Listelenecek geçmiş fatura bulunamadı"
                };
            }

            return new GetPayedInvoiceQueryResponse
            {
                IsSuccess = true,
                Invoices = Invoices,
                Message = "Geçmiş faturalar başarı ile listelendi"
            };
        }
    }
}
