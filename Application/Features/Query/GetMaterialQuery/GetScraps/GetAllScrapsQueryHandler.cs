using Application.DTOs;
using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Query.GetMaterialQuery.GetScraps
{
    public class GetAllScrapsQueryHandler : IRequestHandler<GetAllScrapsQueryRequest, GetAllScrapsQueryResponse>
    {
        private readonly IAppContext _Context;
        private readonly UserManager<AppUser> _userManager;

        public GetAllScrapsQueryHandler(IAppContext Context, UserManager<AppUser> userManager)
        {
            _Context = Context;
            _userManager = userManager;
        }

        public async Task<GetAllScrapsQueryResponse> Handle(GetAllScrapsQueryRequest request, CancellationToken cancellationToken)
        {
            var Owner = await _userManager.FindByIdAsync(request.OwnerId);

            if (Owner == null)
            {
                return new GetAllScrapsQueryResponse
                {
                    IsSucces = false,
                    Scraps = null,
                    Message = "Kullanıcı bilgisine ulaşılamadı.",
                    
                };
            }

            List<ScrapsDto> Scraps = await _Context.Scraps.Where(x => x.OwnerID == Owner.Id).Select(m => new ScrapsDto
            {
                Brand = m.Brand,
                Weight = m.Weight,
                Stock = m.Stock,
                Color = m.Color,
                Height = m.Height,  
                Width = m.Width,
                Thickness = m.Thickness,
                MaterialType = m.MaterialType


            }).AsNoTracking().ToListAsync();

            if (Scraps.Count == 0)
            {
                return new GetAllScrapsQueryResponse
                {
                    IsSucces = true,
                    Scraps =Scraps,
                    Message = "Listelenecek fire ürün bulunamadı"
                };
            }

            return new GetAllScrapsQueryResponse
            {
                IsSucces = true,
                Scraps = Scraps,
                Message = "Fire ürünler başarı ile listelendi"
            };
        }
    }
}
