using Merchants.Application.Responses;
using MediatR;

namespace Merchants.Application.Commands
{
    public class AddUserRequest: IRequest<Response>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserTypeCode { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string? LastUpdateBy { get; set; }
        public string? CustomerCode { get; set; }
        public int? MerchantId { get; set; }
        public string Email { get; set; }
        public int ManagementId { get; set; }
    }
    public class AddUserCommandWithHash : AddUserRequest
    {
        public string SecurityKey { get; set; }
        public string PasswordHash { get; set; }
    }
}
