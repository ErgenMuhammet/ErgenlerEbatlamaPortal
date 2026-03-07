using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.OrderTransactionCommands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommandRequest, DeleteOrderCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppContext _context;

        public DeleteOrderCommandHandler(IAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<DeleteOrderCommandResponse> Handle(DeleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);

            if (Owner == null)
            {
                return new DeleteOrderCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadığından ilan silme işlemi tamamlanamıyor."
                };
            }
            

            var Order = _context.Orders.Where(x => x.OwnerId == Owner.Id && x.Id.ToString() == request.OrderId).FirstOrDefault();

            if (Order == null)
            {
                return new DeleteOrderCommandResponse
                {
                    IsSucces = false,
                    Message = "Ürün bilgisine ulaşılamadı."
                };
            }

            try
            {
                _context.Orders.Remove(Order);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return new DeleteOrderCommandResponse
                {
                    IsSucces = false,
                    Message = $"Sipariş silinirken bir hata ile karşılaşıldı. Hata : {ex.Message}",
                };
            }

            return new DeleteOrderCommandResponse
            {
                IsSucces = true,
                Message = "Sipariş başarı ile silindi",
            };

        }
    }
}
