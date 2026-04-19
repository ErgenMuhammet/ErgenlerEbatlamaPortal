using Application.Features.Command.MaterialTransactionCommand.ReduceGlue;
using Application.Interface;
using Domain.Entitiy.Material;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.ReduceBackPanel
{
    public class ReduceBackPanelCommandHandler : IRequestHandler<ReduceBackPanelCommandRequest, ReduceBackPanelCommandResponse>
    {
        private readonly IAppContext _context;
        private readonly ISignalR _signalR;
        public ReduceBackPanelCommandHandler(IAppContext context, ISignalR signalR)
        {
            _context = context;
            _signalR = signalR;
        }

        public async Task<ReduceBackPanelCommandResponse> Handle(ReduceBackPanelCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var Backpanel = await _context.BackPanel.
                FirstOrDefaultAsync(x => x.OwnerID.ToString() == request.OwnerId && x.Color == request.Color && x.Thickness == request.Thickness && x.Brand == request.Brand);

            if (Backpanel == null)
            {
                return new ReduceBackPanelCommandResponse
                {
                    IsSuccess = false,
                    Message = "İlgili ürün bulunamadı."
                };
            }

            if (Backpanel.Stock == 0)
            {
                return new ReduceBackPanelCommandResponse
                {
                    IsSuccess = true,
                    Message = "İlgili kullanıcıya ait arkalık stoğu bulunmamaktadır."
                };
            }

            if (request.Count > Backpanel.Stock)
            {
                return new ReduceBackPanelCommandResponse
                {
                    IsSuccess = true,
                    Message = "Stoğunuzdan fazla ürün kullanamazsınız."
                };
            }

            Backpanel.Stock -= request.Count;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                return new ReduceBackPanelCommandResponse
                {
                    IsSuccess = true,
                    Message = $"Stoğunuzdan stok güncellenirken bir hata ile karşılaşıldı. Hata : {ex.Message}"
                }; ;
            }
            if (Backpanel.Stock <= 10)
            {
                await _signalR.SendNotification(request.OwnerId!, $"Arkalık stoğu azaldı. Yeni Arkalık Stoğu : {Backpanel.Stock}");

            }
            await _signalR.SendNotification(request.OwnerId!, $"Yeni Arkalık Stoğu : {Backpanel.Stock}");

            return new ReduceBackPanelCommandResponse
            {
                IsSuccess = true,
                Message = "Arkalık stoğu güncellendi"
            };

        }
        
    }
}
