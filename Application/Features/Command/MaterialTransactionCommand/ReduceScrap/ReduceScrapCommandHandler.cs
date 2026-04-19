using Application.Features.Command.MaterialTransactionCommand.ReducePvcBand;
using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.ReduceScrap
{
    public class ReduceScrapCommandHandler : IRequestHandler<ReduceScrapCommandRequest, ReduceScrapCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly ISignalR _signalR;
        public ReduceScrapCommandHandler(IAppContext context, ISignalR signalR)
        {
            _context = context;
            _signalR = signalR;
        }
        public async Task<ReduceScrapCommandResponse> Handle(ReduceScrapCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var Scrap = await _context.Scraps.
                FirstOrDefaultAsync(x => x.OwnerID.ToString() == request.OwnerId && x.Color == request.Color && x.Thickness == request.Thickness);


            if (Scrap.Stock == 0)
            {
                return new ReduceScrapCommandResponse
                {
                    IsSuccess = true,
                    Message = "İlgili kullanıcıya ait PvcBand stoğu bulunmamaktadır."
                };
            }

            if (request.Count > Scrap.Stock)
            {
                return new ReduceScrapCommandResponse
                {
                    IsSuccess = true,
                    Message = "Stoğunuzdan fazla ürün kullanamazsınız."
                };
            }

            Scrap.Stock -= request.Count;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                return new ReduceScrapCommandResponse
                {
                    IsSuccess = true,
                    Message = $"Stok güncellenirken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                }; ;
            }            

            await _signalR.SendNotification(request.OwnerId!, $"Fire ürün kullanıldı");

            return new ReduceScrapCommandResponse
            {
                IsSuccess = true,
                Message = "Fire stoğu güncellendi"
            };
        }
    }
}
