using Merchants.Core.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Merchants.Core.Entities
{
    [Table("tblMerchantLocation")]
    public class MerchantLocations : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? Location { get; set; }
        public int ParentID { get; set; }
        public string? Address { get; set; }
        public string? POCName { get; set; }
        public string? POCEmail { get; set; }
        public string? POCNumber { get; set; }
        public string? OtherEmail { get; set; }
        public string? OtherNumber { get; set; }

    }
}
