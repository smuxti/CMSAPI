using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Commands
{
    public class UpdateUserCommand: IRequest<bool>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string Password { get; set; }
        public int UserTypeCode { get; set; }
    }
}
