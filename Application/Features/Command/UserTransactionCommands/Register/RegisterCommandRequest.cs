using Domain.GlobalEnum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.Register
{
    public class RegisterCommandRequest : IRequest<RegisterCommandResponse>
    {
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Category UserCategory { get; set; }
        public string? Email { get; set; }
        public string?  Password { get; set; }
        public string? PasswordConfirm { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
      
    }
}
