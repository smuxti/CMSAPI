using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Responses
{
    public class AuthResponse
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string MobileNumber { get; set; }
        public int? ManagementId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public List<int> RouteIds { get; set; }
        public bool isUpdated { get; set; } = false;

        //public DateTime RefreshTokenExipryTime { get; set; }
    }
    public class AuthResponseWithKey: AuthResponse
    {
        public Guid Id { get; set; }
        //public string FirstName { get; set; }
        //public string Email { get; set; }
        public string Username { get; set; }
        //public string MobileNumber { get; set; }
        //public string Token { get; set; }
        public string SecurityKey { get; set; }
    }
}
