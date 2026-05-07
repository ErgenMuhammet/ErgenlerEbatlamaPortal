using Application.Features.Command.MaterialTransactionCommand.AddMdf;
using Application.Interface;
using Domain.Entitiy;
using Domain.Entitiy.Material;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.AddGlue
{
    public class AddGlueCommandHandler : IRequestHandler<AddGlueCommandRequest, AddGlueCommandResponse>
    {
        private readonly IAppContext _context;

        public AddGlueCommandHandler( IAppContext context)
        {
            _context = context;
        }
        public async Task<AddGlueCommandResponse> Handle(AddGlueCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.AnyAsync(x => x.Id == request.OwnerId);

            var UserAccounting = await _context.ProfitLossSituation
            .FirstOrDefaultAsync( x => x.OwnerId == request.OwnerId &&
                                  x.Date.Value.Year == DateTime.UtcNow.Year &&
                                  x.Date.Value.Month == DateTime.UtcNow.Month, cancellationToken);

            if (UserAccounting == null)
            {
                return new AddGlueCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı muhasebe bilgilerine ulaşılamadı"
                };
            }

            if (!user)
            {
                return new AddGlueCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı"
                };
            }
            var material = await _context.Glue.FirstOrDefaultAsync(x => x.Brand == request.Brand && x.OwnerID == request.OwnerId);
            try
            {
                if (request.IsPurchase == true)
                {
                    var Purchase = new Expense
                    {
                        Amount = request.Stock * request.UnitPrice ?? 0f,
                        Description = request.SaleDescription,
                        ExpenseDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day),
                        OwnerId = request.OwnerId,
                    };

                    UserAccounting.TotalLoss += Purchase.Amount;
                    UserAccounting.LastSituation = UserAccounting.GetLastSituation();

                    await _context.Expense.AddAsync(Purchase, cancellationToken);
                }

                if (material != null)
                {
                    material.Stock += request.Stock;
                    await _context.SaveChangesAsync(cancellationToken);

                    return new AddGlueCommandResponse
                    {
                        IsSucces = true,
                        Message = "Tutkal stoğu başarıyla güncellendi"
                    };
                }
                
                else 
                {
                    var Glue = new Glue
                    {   OwnerID = request.OwnerId,
                        Brand = request.Brand,                        
                        Stock = request.Stock,
                        Weight = request.Weight,
                    };
                    await _context.Glue.AddAsync(Glue);
                    await _context.SaveChangesAsync(cancellationToken);

                    return new AddGlueCommandResponse
                    {
                        IsSucces = true,
                        Message = "Ürün başarıyla eklendi"
                    };
                }


            }          
            catch (Exception ex)
            {
                return new AddGlueCommandResponse
                {
                    IsSucces = false,
                    Message = $"Tutkal eklenirken bir hata ile karşılaşıldı.Hata : {ex.Message}",
                };
            }

           
        }
    }
}
