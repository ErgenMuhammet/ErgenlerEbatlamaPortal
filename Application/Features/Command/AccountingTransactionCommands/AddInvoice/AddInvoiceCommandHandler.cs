using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Command.AccountingTransaction.AddInvoice
{
    public class AddInvoiceCommandHandler : IRequestHandler< AddInvoiceCommandRequest, AddInvoiceCommandResponse >
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;
        public AddInvoiceCommandHandler(IAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;          
            _userManager = userManager;
        }

        public async Task<AddInvoiceCommandResponse> Handle(AddInvoiceCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.OwnerId);          

            if (user == null)
            {
                return new AddInvoiceCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı"
                };
            }

            var Invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                InvoiceName = request.InvoiceName,  
                Cost = request.Cost,
                OwnerId = request.OwnerId,
                InvoiceNo = request.InvoiceNo,
                LastPaymentDate = request.LastPaymentDate,
                Owner = user,
            };

            var IsContain = await _context.Invoice.AnyAsync(x => x.InvoiceNo == request.InvoiceNo && 
                                                                  x.InvoiceName == request.InvoiceName &&
                                                                        x.OwnerId == request.OwnerId);
            if (IsContain)
            {
                return new AddInvoiceCommandResponse
                {
                    IsSucces = false,
                    Message = "Bu fatura zaten listenizde mevcut."
                };
            }

            await _context.Invoice.AddAsync(Invoice);

            await _context.Expense.AddAsync(new Expense
                   {
                        OwnerId = request.OwnerId,
                        Amount = request.Cost,
                        ExpenseDate = request.LastPaymentDate,
                        Description = "Fatura Ödemesi"
                   });

           
                      

            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                return new AddInvoiceCommandResponse
                {
                    IsSucces = true,
                    Message = "Fatura başarıyla eklendi."
                    
                };
            }

             return new AddInvoiceCommandResponse
            {
                 IsSucces = false,
                 Message = "Fatura eklenirken bir hatayla karşılaşıldı daha sonra tekrar deneyiniz."
             };

        }
    }
}
