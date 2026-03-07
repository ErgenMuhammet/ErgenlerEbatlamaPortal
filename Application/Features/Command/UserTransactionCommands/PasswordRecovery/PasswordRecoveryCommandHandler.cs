using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.PasswordRecovery
{
    public class PasswordRecoveryCommandHandler : IRequestHandler<PasswordRecoveryCommandRequest, PasswordRecoveryCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public PasswordRecoveryCommandHandler(UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<PasswordRecoveryCommandResponse> Handle(PasswordRecoveryCommandRequest request, CancellationToken cancellationToken)
        {
           var user = await _userManager.FindByEmailAsync(request.Email);
           var passwordtoken = await _userManager.GeneratePasswordResetTokenAsync(user);
           var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordtoken));


            if (user == null)
            {
                return new PasswordRecoveryCommandResponse
                {
                    Message = "Bu email adresi ile eşleşen bir kullanıcı bulunamadı."
                };
            }

            string baseUrl = "http://localhost:5233";
            var confirmationLink = $"{baseUrl}/Portal/ValidateToken?userId={user.Id}&token={encodedToken}";
            try
            {
               

                await _emailService.SendEmail(
                   user.Email,
                   "ErgenlerPortal Şifre Sıfırlama",
                   $"<h3>Hoşgeldiniz {user.FullName}!</h3><p>Hesabınızı doğrulamak ve giriş yapabilmek için lütfen <a href='{confirmationLink}'>buraya tıklayın</a>.</p>"
               );

            }
            catch (Exception ex)
            {
                return new PasswordRecoveryCommandResponse
                { 
                    Message = ex.Message,
                    IsSucces = false
                };               
            }

            return new PasswordRecoveryCommandResponse
            {
                Message = "Email adresinize şifre sıfırlama linkiniz gönderilmiştir.İşleminize oradan devam edebilirsiniz.",
                IsSucces = true
            };


        }
    }
}
