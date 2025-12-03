using Merchants.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchants.Core.Entities
{
    [Table("tblTenant")]
    public class Tenant: BaseEntity
    {
        public string Name { get; set; }
        public string TenantCode { get; set; }
    }
}
