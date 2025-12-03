using Merchants.Core.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Merchants.Core.Entities
{
    [Table("tblMerchant")]
    public class Merchant: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }
        public string MerchantAddress { get; set; }

        public string? Email { get; set; }
        public string? OtherEmail { get; set; }
        public string? Number { get; set; }
        public string? OtherNumber { get; set; }
        public string City { get; set; }

        public int Area { get; set; }
        public int Zone { get;set; }
        //public int ZoneID { get; set; }

        //public DateTime CreatedDate { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public int? UpdateBy { get; set; }
        //public DateTime? DeleteDate { get; set; }
        //public int? DeleteBy { get; set; }
        //public bool? IsDelete { get; set; }

        //public string? Remarks { get; set; }
    }
}
