using Application.DTOs;
using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.GetMyChatList
{
    public class GetMyChatListHandler : IRequestHandler<GetMyChatListRequest, GetMyChatListResponse>
    {
        private readonly IAppContext _context;
        public GetMyChatListHandler(IAppContext context)
        {
            _context = context;
        }
        public async Task<GetMyChatListResponse> Handle(GetMyChatListRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var ChatList = await _context.Messages.
            Where(x => x.ReceiverId == request.OwnerId || x.SenderId == request.OwnerId).Select(x => x.SenderId == request.OwnerId ? x.ReceiverId : x.SenderId).
            Distinct().ToListAsync(cancellationToken);

            var Users = await _context.AppUsers.Where(x => ChatList.Contains(x.Id.ToString())).
            Select(x => new MessageListDto
            {
                    Id = x.Id,
                    FullName = x.FullName

            }).ToListAsync(cancellationToken);

            if (ChatList.Count == 0 || ChatList == null)
            {
                return new GetMyChatListResponse
                {
                    ChatList = null,
                    IsSuccess = false,
                    Message = "Mesaj kutunuz boş"
                };
            }

            return new GetMyChatListResponse
            {
                Message = "Mesaj listesi başarıyla getirildi.",
                IsSuccess = true,
                ChatList = Users
            };
        }
    }
}
