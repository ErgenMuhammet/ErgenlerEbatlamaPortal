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

namespace Application.Features.Query.GetUserTransactionQuery.GetMyPastMessage
{
    public class GetMyPastMessageHandler : IRequestHandler<GetMyPastMessageRequest, GetMyPastMessageResponse>
    {
        private readonly IAppContext _context;
        private readonly UserManager<AppUser> _userManager;
        public GetMyPastMessageHandler(IAppContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<GetMyPastMessageResponse> Handle(GetMyPastMessageRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentException(nameof(request));
            }


            var Messages = await _context.Messages.
                Where(x => (x.SenderId == request.OwnerId && x.ReceiverId == request.TargetId) || (x.SenderId == request.TargetId && x.ReceiverId == request.OwnerId)).
                Select(x=> new MessageDto
                {
                    Content = x.Content,
                    SendAt = x.SendAt,
                    IsMine= x.SenderId == request.OwnerId 

                }). OrderByDescending(x => x.SendAt).ToListAsync(cancellationToken);
            
            if (Messages.Count == 0 || Messages == null)
            {
                return new GetMyPastMessageResponse
                {
                    IsSuccess = false,
                    Message = "Sohbet geçmişi bulunmamaktadır.",
                    Messages = null
                };
            }

            return new GetMyPastMessageResponse
            {
                IsSuccess = true,
                Message = "Sohbet geçmişi başarıyla getirildi.",
                Messages = Messages
            };

          
        }
    }
}
