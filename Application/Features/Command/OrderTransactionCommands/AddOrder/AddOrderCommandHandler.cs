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

namespace Application.Features.Command.OrderTransactionCommands.AddOrder
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommandRequest, AddOrderCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AddOrderCommandHandler(IAppContext context , UserManager<AppUser> userManager) 
        {
                _context = context;
                _userManager = userManager;
        }

        public async  Task<AddOrderCommandResponse> Handle(AddOrderCommandRequest request, CancellationToken cancellationToken)
        {
           var Owner = await _userManager.FindByIdAsync(request.OwnerId);

            if (Owner == null)
            {
                return new AddOrderCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı.Daha sonra tekrar deneyiniz."
                };
            }

            var Order = new Order
            {
                Owner = Owner,
                CostOfOrder = request.CostOfOrder,
                CountOfBackPanel = request.CountOfBackPanel,
                MetreOfPvcBand = request.MetreOfPvcBand,
                OrderName = request.OrderName,
                OwnerId = request.OwnerId,
                CountOfMdf = request.CountOfMdf,
            };

            await _context.AddAsync(Order,cancellationToken);

            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                return new AddOrderCommandResponse
                {
                    IsSucces = true,
                    Message = "Sipariş başarıyla eklendi"
                };
            }

            return new AddOrderCommandResponse
            {
                 IsSucces = false,
                 Message = "Sipariş eklenirken bir sorun oluştu.Daha sonra tekrar deneyiniz"
            };

        }
    }
}
