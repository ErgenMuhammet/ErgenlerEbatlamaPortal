using Application.DTOs;
using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.GetUserProporties
{
    public class GetUserProportiesHandler : IRequestHandler<GetUserProportiesRequest, GetUserProportiesResponse>
    {
        private readonly IAppContext _context;
        public GetUserProportiesHandler(IAppContext context)
        {
            _context = context;          
        }

        public async Task<GetUserProportiesResponse> Handle(GetUserProportiesRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = await _context.AppUsers.Include(x => x.Jobs).Where(x => x.Id.ToString() == request.OwnerId).Select(x => new UserProportiesDto
            {
                AdressDescription = x.Jobs.AdressDescription,
                BirthDate = x.BirthDate,
                Email = x.Email,
                Experience = x.Jobs.Experience,
                FullName = x.FullName,
                PhoneNumber = x.Jobs.PhoneNumber,
                WorkPhoneNumber = x.Jobs.PhoneNumber,
                WorkShopName = x.Jobs.WorkShopName,
                City = x.City

            }).FirstOrDefaultAsync();

            if (user == null)
            {
                return new GetUserProportiesResponse
                {
                    IsSucces = false,
                    Message = "İlgili kullanıcı bulunamadı",
                    User = null
                };
            }

            return new GetUserProportiesResponse
            {
                IsSucces = true,
                Message = "Kullanıcı bilgileri başarıyla getirildi",
                User = user
            };
        }
    }
}
