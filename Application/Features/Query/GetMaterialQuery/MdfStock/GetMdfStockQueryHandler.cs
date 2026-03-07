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

namespace Application.Features.Query.GetMaterialQuery.MdfStock
{
    public class GetMdfStockQueryHandler : IRequestHandler<GetMdfStockQueryRequest, GetMdfStockQueryResponse>
    {
        private readonly IAppContext _Context;
        private readonly UserManager<AppUser> _userManager;

        public GetMdfStockQueryHandler(IAppContext Context , UserManager<AppUser> userManager)
        {
            _Context = Context;
            _userManager = userManager;
        }

        public async Task<GetMdfStockQueryResponse> Handle(GetMdfStockQueryRequest request, CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);

            if (Owner == null)
            {
                return new GetMdfStockQueryResponse
                {
                    IsSucces = false,
                    Mdf = null,
                    Message = "Kullanıcı bilgisine ulaşılamadı."
                };
            }

            List<MdfDto> Mdfs = await _Context.Mdf.Where(x => x.OwnerID == Owner.Id).Select(m => new MdfDto
            {
                Brand = m.Brand,
                Color = m.Color,
                Stock = m.Stock,
                Thickness = m.Thickness,
                Weight = m.Weight,

            }).AsNoTracking().ToListAsync();

            if (Mdfs.Count == 0)
            {
                return new GetMdfStockQueryResponse
                {
                    IsSucces = true,
                    Mdf = Mdfs,
                    Message = "Listelenecek ürün bulunamadı"
                };
            }

            return new GetMdfStockQueryResponse
            {
                IsSucces = true,
                Mdf = Mdfs,
                Message = "ürünler başarı ile listelendi"
            };    
        }                       

    }
}
