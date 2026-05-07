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
                    Message = "Kullanıcı bilgisine ulaşılamadı.",

                };
            }

            List<InvoiceDto> ElectricInvoices = await _context.Invoice.Where(x => x.OwnerId == Owner.Id && x.BeenPaid == false && x.InvoiceType == "Electric").Select(m => new InvoiceDto
                {
                    Id = m.Id.ToString(),
                    LastPaymentDate = m.LastPaymentDate,
                    Name = m.InvoiceName,
                    InvoicesNo = m.InvoiceNo,
                    Price = m.Cost,
                    BeenPaid = m.BeenPaid,
                InvoiceType = m.InvoiceType ?? "Electric",

            }).AsNoTracking().OrderByDescending(x => x.LastPaymentDate).ToListAsync();

            List<InvoiceDto> WaterInvoices = await _context.Invoice.Where(x => x.OwnerId == Owner.Id && x.BeenPaid == false && x.InvoiceType == "Water").Select(m => new InvoiceDto
                {
                    Id = m.Id.ToString(),
                    LastPaymentDate = m.LastPaymentDate,
                    Name = m.InvoiceName,
                    InvoicesNo = m.InvoiceNo,
                    Price = m.Cost,
                    BeenPaid = m.BeenPaid,
                InvoiceType = m.InvoiceType ?? "Water",

            }).AsNoTracking().OrderByDescending(x => x.LastPaymentDate).ToListAsync();

            List<InvoiceDto> NaturalGasInvoices = await _context.Invoice.Where(x => x.OwnerId == Owner.Id && x.BeenPaid == false && x.InvoiceType == "NaturalGas" ).Select(m => new InvoiceDto
                {
                    Id = m.Id.ToString(),
                    LastPaymentDate = m.LastPaymentDate,
                    Name = m.InvoiceName,
                    InvoicesNo = m.InvoiceNo,
                    Price = m.Cost,
                    BeenPaid = m.BeenPaid,
                InvoiceType = m.InvoiceType ?? "NaturalGas",

            }).AsNoTracking().OrderByDescending(x => x.LastPaymentDate).ToListAsync();

            List<InvoiceDto> OtherInvoices = await _context.Invoice.Where(x => x.OwnerId == Owner.Id && x.BeenPaid == false && x.InvoiceType == "Other" ).Select(m => new InvoiceDto
                {
                    Id = m.Id.ToString(),
                    LastPaymentDate = m.LastPaymentDate,
                    Name = m.InvoiceName,
                    InvoicesNo = m.InvoiceNo,
                    Price = m.Cost,
                    BeenPaid = m.BeenPaid,
                InvoiceType = m.InvoiceType ?? "Other",

            }).AsNoTracking().OrderByDescending(x => x.LastPaymentDate).ToListAsync();

            List<InvoiceDto> PaidInvoices = await _context.Invoice.Where(x => x.OwnerId == Owner.Id && x.BeenPaid == true).Select(m => new InvoiceDto
                {
                    Id = m.Id.ToString(),
                    LastPaymentDate = m.LastPaymentDate,
                    Name = m.InvoiceName,
                    InvoicesNo = m.InvoiceNo,
                    Price = m.Cost,
                    BeenPaid = m.BeenPaid,
                    InvoiceType = m.InvoiceType ?? "Other",

            }).AsNoTracking().ToListAsync();


            return new GetAllInvoicesQueryResponse
            {
                IsSucces = true,
                ElectricInvoices = ElectricInvoices,
                NaturalGasInvoices = NaturalGasInvoices,
                WaterInvoices = WaterInvoices,
                OtherInvoices = OtherInvoices,
                PaidInvoice = PaidInvoices,
                Message = "Faturalar başarı ile listelendi"
            };
        }
    }
}