using Application.Interface;
using Domain.Entitiy;
using Domain.GlobalEnum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IAppContext _context;
        public RegisterCommandHandler(UserManager<AppUser> userManager, IEmailService emailService, IAppContext context)
        {
            _userManager = userManager;
            _emailService = emailService;
            _context = context;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
        {
            
            var user = new AppUser
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                BirthDate = request.BirthDate,
                UserName = request.UserName,
                City = request.City,

                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                UserCategory = request.UserCategory,
            };

            if (request.Password == request.PasswordConfirm)
            {
                await _userManager.CreateAsync(user, request.Password);

                var UserAccounting = new ProfitLossSituation
                {
                    Date = DateTime.UtcNow,
                    OwnerId = user.Id,
                    TotalLoss = 0,
                    TotalProfit = 0,
                    LastSituation = 0,
                };

                var result = await _context.SaveChangesAsync(cancellationToken);

                if (result >= 2)   
                {
                    try
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        var encodedtoken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                        string baseUrl = "http://localhost:5233";

                        var confirmationLink = $"{baseUrl}/Portal/ConfirmEmail?userId={user.Id}&token={encodedtoken}";

                        await _emailService.SendEmail(
                           user.Email,
                           "ErgenlerPortal Hesap Doğrulama",
                           $"<h3>Hoşgeldiniz {user.FullName}!</h3><p>Hesabınızı doğrulamak ve giriş yapabilmek için lütfen <a href='{confirmationLink}'>buraya tıklayın</a>.</p>"
                       );
                    }

                    catch (Exception ex)
                    {

                        return new RegisterCommandResponse
                        {
                            IsSucces = false,
                            Message = "Kullanıcı oluşturulurken email servisinde hata meydana geldi.",
                            Errors = ex.Message
                        };
                    }
                    return new RegisterCommandResponse
                    {
                        Message = "Lütfen email adresinize gelen doğrulama linkine tıklayınız.",
                        IsSucces = true
                    };
                }
                else 
                {
                    return new RegisterCommandResponse
                    {
                        Message = "Hesap oluşturulurken bir hata oluştu lütfen tekrar deneyiniz",
                        IsSucces = false
                    };
                }
               
            }
            else {
                return new RegisterCommandResponse
                {
                    Message = "Şifreler uyuşmuyor tekrar deneyiniz.",
                    IsSucces = false
                };
            }
        }
    }
}
