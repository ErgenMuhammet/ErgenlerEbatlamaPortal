using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.OrderTransactionCommands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommandRequest, UpdateOrderCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UpdateOrderCommandHandler(IAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;          
        }

        public async Task<UpdateOrderCommandResponse> Handle(UpdateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);
            var Order = await _context.Orders.Where(x => x.Id.ToString() == request.OrderId && x.OwnerId == request.OwnerId).FirstOrDefaultAsync();

            if (Order == null)
            {
                return new UpdateOrderCommandResponse
                {
                    IsSuccess = false,
                    Message = "Ürün bilgisine ulaşılamadı.Daha sonra tekrar deneyiniz."
                };
            }

            if (Owner == null)
            {
                return new UpdateOrderCommandResponse
                {
                    IsSuccess = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı.Daha sonra tekrar deneyiniz."
                };
            }

            Order.CountOfMdf = request.CountOfMdf;
            Order.OrderName = request.OrderName;    
            Order.CostOfOrder = request.CostOfOrder;    
            Order.CountOfBackPanel = request.CountOfBackPanel;
            Order.MetreOfPvcBand = request.MetreOfPvcBand;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return new UpdateOrderCommandResponse
                {
                    IsSuccess = false,
                    Message = $"Ürün güncellenirken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                };
            }

            return new UpdateOrderCommandResponse
            {
                IsSuccess = true,
                Message = "Ürün başarıyla güncellendi."
            };

        }
    }
}

