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

namespace Application.Features.Command.OrderTransactionCommands.OrderIsDone
{
    public class OrderIsDoneCommandHandler :IRequestHandler<OrderIsDoneCommandRequest,OrderIsDoneCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppContext _context;

        public OrderIsDoneCommandHandler(UserManager<AppUser> userManager, IAppContext context)
        {         
            _userManager = userManager;
            _context = context;           
        }

        public async Task<OrderIsDoneCommandResponse> Handle(OrderIsDoneCommandRequest request,CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);

            if (Owner == null)
            {
                return new OrderIsDoneCommandResponse
                {
                    IsSuccess = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı."
                };
            }

            var Order = await _context.Orders.Where(x => x.OwnerId == request.OwnerId && x.Id.ToString() == request.OrderId).FirstOrDefaultAsync();

            if (Order == null)
            {
                return new OrderIsDoneCommandResponse
                {
                    IsSuccess = false,
                    Message = "Sipariş bilgisine ulaşılamadı."
                };
            }

            try
            {
                Order.IsDone = true;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                return new OrderIsDoneCommandResponse
                {
                    IsSuccess = false,
                    Message = $"İşlem yapılırken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                };
            }

            return new OrderIsDoneCommandResponse
            {
                IsSuccess = true,
                Message = "İşlem gerçekleşti"
            };

        }
    }
}
