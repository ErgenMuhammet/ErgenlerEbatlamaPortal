using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.AccountingTransactionCommands.UpdateInvoice
{
    public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommandRequest, UpdateInvoiceCommandResponse>
    {
        private readonly IAppContext _context;
        public UpdateInvoiceCommandHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<UpdateInvoiceCommandResponse> Handle(UpdateInvoiceCommandRequest request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoice.FirstOrDefaultAsync(x => x.Id .ToString() ==request.InvoiceId);
            if (invoice == null)
            {
                return new UpdateInvoiceCommandResponse
                {
                    IsSucces = false,
                    Message = "İlgili fatura bulunamadı"
                };
            }

            var User = await _context.AppUsers.AnyAsync (x => x.Id == invoice!.OwnerId);

            if (invoice.OwnerId != request.OwnerId)
            {
                return new UpdateInvoiceCommandResponse
                {
                    IsSucces = false,
                    Message = "İlgili fatura mevcut kullanıcıya ait değildir."
                };
            }

            if (!User)
            {
                return new UpdateInvoiceCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgileri bulunamadı."
                };
            }

            try
            {
                invoice.InvoiceNo = request.InvoiceNo;
                invoice.InvoiceName = request.InvoiceName;

                await _context.SaveChangesAsync(cancellationToken);
                
                return new UpdateInvoiceCommandResponse
                {
                    IsSucces = false,
                    Message = "Fatura başarıyla güncellendi."
                };
            }
            catch (Exception ex)
            {
                return new UpdateInvoiceCommandResponse
                {
                    IsSucces = false,
                    Message = $"Fatura güncellenirken bir hata ile karşılaşıldı.Hata : {ex.Message}"
                };
            }
        
        
        }
    }
}
