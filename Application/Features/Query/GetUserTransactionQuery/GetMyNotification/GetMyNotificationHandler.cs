using Application.DTOs;
using Application.Interface;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.GetMyNotification
{
    public class GetMyNotificationHandler : IRequestHandler <GetMyNotificationRequest,GetMyNotificationResponse>
    {
        private readonly IAppContext _context;

        public GetMyNotificationHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<GetMyNotificationResponse> Handle(GetMyNotificationRequest request,CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var notifications = await _context.notifications.Where(x => x.OwnerId == request.OwnerId).Select( x => new NotificationDto
            {
                Content = x.Content,
            }).ToListAsync(cancellationToken);

            if (notifications.Count == 0 || notifications == null)
            {
                return new GetMyNotificationResponse
                {
                    IsSuccess = false,
                    Message = "Henüz bildiriminiz yok.",
                    Notifications = null
                };
            }

            return new GetMyNotificationResponse
            {
                IsSuccess = true,
                Message = "Bildirimler başarıyla görüntülendi.",
                Notifications = notifications  
            };
        }
    }
}
