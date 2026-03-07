using Application.DTOs;
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
//DENEME@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
namespace Application.Features.Query.GetMaterialQuery.BackPanelStock
{
    public class GetBackPanelStockQueryHandler : IRequestHandler<GetBackPanelStockQueryRequest, GetBackPanelStockQueryResponse>
    {
        private readonly IAppContext _Context;
        private readonly UserManager<AppUser> _userManager;

        public GetBackPanelStockQueryHandler(IAppContext Context, UserManager<AppUser> userManager)
        {
            _Context = Context;
            _userManager = userManager;
        }
        public async Task<GetBackPanelStockQueryResponse> Handle(GetBackPanelStockQueryRequest request, CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);

            if (Owner == null)
            {
                return new GetBackPanelStockQueryResponse
                {
                    IsSucces = false,
                    BackPanel = null,
                    Message = "Kullanıcı bilgisine ulaşılamadı."
                };
            }

            List<BackPanelDto> BackPanels = await _Context.BackPanel.Where(x => x.OwnerID == Owner.Id).Select(m => new BackPanelDto
            {
                Color = m.Color,
                Brand = m.Brand,
                Thickness = m.Thickness,
                Stock = m.Stock,


            }).AsNoTracking().ToListAsync();

            if (BackPanels.Count == 0)
            {
                return new GetBackPanelStockQueryResponse
                {
                    IsSucces = true,
                     BackPanel= BackPanels,
                    Message = "Listelenecek ürün bulunamadı"
                };
            }

            return new GetBackPanelStockQueryResponse
            {
                IsSucces = true,
                BackPanel = BackPanels,
                Message = "ürünler başarı ile listelendi"
            };
        }

    }
}
