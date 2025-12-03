using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands
{
    public class UpdateUserCommand: IRequest<Response>
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? PasswordHash { get; set; }
        public string SecurityKey { get; set; }
        public int UserTypeCode { get; set; }
        public int? MerchantId { get; set; }
        public string Email { get; set; }
        public int ManagementId { get; set; }
        public string? LastUpdateBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
