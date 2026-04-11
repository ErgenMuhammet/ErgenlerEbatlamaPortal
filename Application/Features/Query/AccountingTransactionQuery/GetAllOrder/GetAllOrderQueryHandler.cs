using Application.DTOs;
using Application.Interface;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entitiy;

namespace Application.Features.Query.AccountingTransactionQuery.GetAllOrder
{    
    public class GetAllIncomesQueryHandler : IRequestHandler<GetAllIncomesQueryRequest, GetAllIncomesQueryResponse>
    {
        private readonly IAppContext _context ;
        private readonly UserManager<AppUser> _userManager;

        public GetAllIncomesQueryHandler(IAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<GetAllIncomesQueryResponse> Handle(GetAllIncomesQueryRequest? request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.OwnerId);

            if (user == null)
            {
                return new GetAllIncomesQueryResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı",
                    Orders = null
                };
            }

            var orders = await _context.Orders.Where(x => x.OwnerId == request.OwnerId).Select(x => new OrdersDto
            {
            //    CostOfOrder = x.CostOfOrder,
                CountOfBackPanel = x.CountOfBackPanel,
                CountOfMdf = x.CountOfMdf,
                MetreOfPvcBand = x.MetreOfPvcBand,
                OrderDate = x.OrderDate,
                //OrderName = x.OrderName,
            }).AsNoTracking().ToListAsync(cancellationToken);

            if (orders.Count > 0)
            {
                return new GetAllIncomesQueryResponse
                {
                    IsSucces = true,
                    Message = "Sipariş listesi başarıyla getirildi",
                    Orders = orders
                };
            }
            else if (orders == null || orders.Count ==  0)                      
            {
                return new GetAllIncomesQueryResponse
                {
                    IsSucces = true,
                    Message = "Görüntülenecek bir sipariş bulunamadı",
                    Orders = null
                };
            }

            return new GetAllIncomesQueryResponse
            {
                IsSucces = true,
                Message = "Siparişler listelenirken bir hata oldu.Daha sonra tekrar deneyiniz",
                Orders = orders
            };
        }
    }
}
