using Application.Features.Command.MaterialTransactionCommand.UpdatePvcBand;
using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.UpdateScrap
{
    public class UpdateScrapCommandHandler : IRequestHandler<UpdateScrapCommandRequest, UpdateScrapCommandResponse>
    {
        private readonly IAppContext _context;

        public UpdateScrapCommandHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<UpdateScrapCommandResponse> Handle(UpdateScrapCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.AnyAsync(x => x.Id.ToString() == request.OwnerId);

            if (!user)
            {
                return new UpdateScrapCommandResponse
                {
                    IsSuccess = false,
                    Message = "Mevcut kullanıcı bilgilerine ulaşılamadı"
                };
            }
            var Scrap = await _context.Scraps.FirstOrDefaultAsync(x => x.Id.ToString() == request.ScrapId);

            if (Scrap.OwnerID == request.OwnerId)
            {
                return new UpdateScrapCommandResponse
                {
                    IsSuccess = false,
                    Message = "Mevcut ürün ilgili kişiye ait değildir."
                };
            }
                                       
                    Scrap.Brand = request.Brand;
                    Scrap.Width = request.Width;
                    Scrap.Thickness = request.Thickness;
                    Scrap.Color = request.Color;
                    Scrap.Weight = request.Weight;
                    Scrap.Height = request.Height;

                    var result = await _context.SaveChangesAsync(cancellationToken);

            if (result > 0 )
            {
                    return new UpdateScrapCommandResponse
                    {
                        IsSuccess = false,
                        Message = "İlgili ürün güncellendi"
                    };
                
            }

                return new UpdateScrapCommandResponse
                {
                    IsSuccess = false,
                    Message = "İlgili ürün güncellenirken bir hata ile karşılaşıldı."
                };
            
        }
    }
}
