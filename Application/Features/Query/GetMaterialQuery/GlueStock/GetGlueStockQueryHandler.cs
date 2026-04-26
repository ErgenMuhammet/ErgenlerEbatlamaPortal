using Application.DTOs;
using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Query.GetMaterialQuery.GlueStock
{
    public class GetGlueStockQueryHandler : IRequestHandler<GetGlueStockQueryRequest, GetGlueStockQueryResponse>
    {
        private readonly IAppContext _Context;
        private readonly UserManager<AppUser> _userManager;

        public GetGlueStockQueryHandler(IAppContext Context, UserManager<AppUser> userManager)
        {
            _Context = Context;
            _userManager = userManager;
        }
        public async Task<GetGlueStockQueryResponse> Handle(GetGlueStockQueryRequest request, CancellationToken cancellationToken)
        {

            var Owner = await _userManager.FindByIdAsync(request.OwnerId);

            if (Owner == null)
            {
                return new GetGlueStockQueryResponse
                {
                    IsSucces = false,
                    Glue = null,
                    Message = "Kullanıcı bilgisine ulaşılamadı."
                };
            }

            List<GlueDto> Glues = await _Context.Glue.Where(x => x.OwnerID == Owner.Id).Select(m => new GlueDto
            {
                Id = m.Id.ToString(),   
                Brand = m.Brand,
                Weight = m.Weight,
                Stock = m.Stock,
                

            }).AsNoTracking().ToListAsync();

            if (Glues.Count == 0)
            {
                return new GetGlueStockQueryResponse
                {
                    IsSucces = true,
                    Glue = Glues,
                    Message = "Listelenecek ürün bulunamadı"
                };
            }

            return new GetGlueStockQueryResponse
            {
                IsSucces = true,
                Glue = Glues,
                Message = "ürünler başarı ile listelendi"
            };
        }
    }
}
