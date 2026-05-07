using Application.DTOs;
using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.AccountingTransactionQuery.GetPayedInvoice
{
    public class GetPayedInvoiceQueryHandler : IRequestHandler<GetPayedInvoiceQueryRequest, GetPayedInvoiceQueryResponse>
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;
        public GetPayedInvoiceQueryHandler(IAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<GetPayedInvoiceQueryResponse> Handle(GetPayedInvoiceQueryRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException();
            }

            var Invoices = await _context.Invoice.Where(x => x.OwnerId == request.OwnerId && x.BeenPaid == true).Select(x => new InvoiceDto
            {
                Id = x.Id.ToString(),
                InvoicesNo = x.InvoiceNo,
                LastPaymentDate = x.LastPaymentDate,    
                Name = x.InvoiceName,
                Price = x.Cost,
                InvoiceType = x.InvoiceType }).ToListAsync(cancellationToken);


            if (Invoices.Count == 0)
            {
                return new GetPayedInvoiceQueryResponse
                {
                    Invoices = Invoices,
                };
            }

            return new GetPayedInvoiceQueryResponse
            {
                Invoices = Invoices,
                IsSuccess = true,
                Message = "Ödenmiş faturalar getirildi."
            };
        }
    }
}
