using Authentication.Application.Responses;
using MediatR;

namespace Authentication.Application.Commands
{
    public class AddUserRequest: IRequest<Response>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Passowrd { get; set; } 
        public string? TenantCode { get; set; }
        public Guid MerchantId { get; set; }
        public Guid CustomerId { get; set; }
        public int UserTypeCode { get; set; }
    }
    public class AddUserCommandWithHash : AddUserRequest
    {
        public string SecurityKey { get; set; }
        public string PasswordHash { get; set; }
    }
}
