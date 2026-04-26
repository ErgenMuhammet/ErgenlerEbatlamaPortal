using Application.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Resend;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IResend _resend;
        public EmailService(IConfiguration configuration, IResend resend)
        {
            _configuration = configuration;
            _resend = resend;            
        }

        public async Task SendEmail(string toUser, string Subject, string htmlbody)
        {
            
            var from = _configuration["ResendEmail:FromEmail"];
            var msg = new EmailMessage();
            msg.From = from;
            msg.To.Add(toUser);
            msg.Subject = Subject;
            msg.HtmlBody = htmlbody;
          
            try
            {
               await _resend.EmailSendAsync(msg);
            }
            catch (Exception ex)
            {
                throw new Exception($"Email gönderilirken bir hata ile karşılaşıldı. Hata : {ex.Message}");
            }
            
        }

    }
}
