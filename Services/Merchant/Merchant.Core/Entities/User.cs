using Merchants.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchants.Core.Entities
{
    [Table("tblUser")]
    public class User: BaseEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityKey { get; set; }
        public int UserTypeCode { get; set; }
        public int? MerchantId { get; set; }
        public int ManagementId { get; set; }
        public string? LastUpdateBy { get; set; }
        public string? RefreshToken { get; set; }
        public string Email { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
