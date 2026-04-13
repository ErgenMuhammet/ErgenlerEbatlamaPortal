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

namespace Application.Features.Query.GetMaterialQuery.PvcBandStock
{
    public class GetPvcBandStockQueryHandler : IRequestHandler<GetPvcBandStockQueryRequest, GetPvcBandStockQueryResponse>
   {
        private readonly IAppContext _Context;
        private readonly UserManager<AppUser> _userManager;

        public GetPvcBandStockQueryHandler(IAppContext Context, UserManager<AppUser> userManager)
        {
            _Context = Context;
            _userManager = userManager;
        }
        public async Task<GetPvcBandStockQueryResponse> Handle(GetPvcBandStockQueryRequest request, CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);

            if (Owner == null)
            {
                return new GetPvcBandStockQueryResponse
                {
                    IsSucces = false,
                    PvcBand = null,
                    Message = "Kullanıcı bilgisine ulaşılamadı."
                };
            }

            List<PvcBandDto> PvcBandDto = await _Context.PvcBand.Where(x => x.OwnerID == Owner.Id).Select(m => new PvcBandDto
            {
                Color = m.Color,
                Brand = m.Brand,
                Thickness = m.Thickness,
                Stock = m.Stock,


            }).AsNoTracking().ToListAsync();

            if (PvcBandDto.Count == 0)
            {
                return new GetPvcBandStockQueryResponse
                {
                    IsSucces = true,
                    PvcBand = PvcBandDto,
                    Message = "Listelenecek ürün bulunamadı"
                };
            }

            return new GetPvcBandStockQueryResponse
            {
                IsSucces = true,
                PvcBand = PvcBandDto,
                Message = "ürünler başarı ile listelendi"
            };
        }
    }
}
