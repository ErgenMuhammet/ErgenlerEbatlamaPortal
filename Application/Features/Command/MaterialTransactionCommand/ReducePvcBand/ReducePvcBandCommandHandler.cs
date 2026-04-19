using Application.Features.Command.MaterialTransactionCommand.ReduceBackPanel;
using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.ReducePvcBand
{
    public class ReducePvcBandCommandHandler : IRequestHandler<ReducePvcBandCommandRequest, ReducePvcBandCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly ISignalR _signalR;
        public ReducePvcBandCommandHandler(IAppContext context, ISignalR signalR)
        {
            _context = context;
            _signalR = signalR;
        }
        public async Task<ReducePvcBandCommandResponse> Handle(ReducePvcBandCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var PvcBand = await _context.PvcBand.
                FirstOrDefaultAsync(x => x.OwnerID.ToString() == request.OwnerId && x.Color == request.Color && x.Thickness == request.Thickness && x.Brand == request.Brand);

            if (PvcBand == null)
            {
                return new ReducePvcBandCommandResponse
                {
                    IsSuccess = false,
                    Message = "İlgili ürün bulunamadı."
                };
            }


            if (PvcBand.Stock == 0)
            {
                return new ReducePvcBandCommandResponse
                {
                    IsSuccess = true,
                    Message = "İlgili kullanıcıya ait PvcBand stoğu bulunmamaktadır."
                };
            }

            if (request.Count > PvcBand.Stock)
            {
                return new ReducePvcBandCommandResponse
                {
                    IsSuccess = true,
                    Message = "Stoğunuzdan fazla ürün kullanamazsınız."
                };
            }

            PvcBand.Stock -= request.Count;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                return new ReducePvcBandCommandResponse
                {
                    IsSuccess = true,
                    Message = $"Stoğunuzdan stok güncellenirken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                }; ;
            }

            if (PvcBand.Stock <= 10)
            {
                await _signalR.SendNotification(request.OwnerId!, $" PvcBand Stoğu Azaldı.Yeni Stok : {PvcBand.Stock}");
            }

                await _signalR.SendNotification(request.OwnerId!, $"Yeni PvcBand Stoğu : {PvcBand.Stock}");

            return new ReducePvcBandCommandResponse
            {
                IsSuccess = true,
                Message = "PvcBand stoğu güncellendi"
            };
        }
    }
}
