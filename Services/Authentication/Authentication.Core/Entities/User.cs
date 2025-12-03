using Authentication.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Core.Entities
{
    [Table("tblUser")]
    public class User: BaseEntity
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityKey { get; set; }
        public int UserTypeCode { get; set; }
        public Guid MerchantId { get; set; }
        public Guid CustomerId { get; set; }
        public string TenantCode { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
