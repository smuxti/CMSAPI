using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchants.Core.Common;
using System.Text.Json.Serialization;

namespace Merchants.Core.Entities
{
    [Table("tblComplaint")]
    public class Complaint : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        //public string? TickentNo { get; set; }
        public DateTime? ComplaintDate { get; set; }
        public int ComplainerID { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public int TypeID { get; set; }
        public int CategoryID { get; set; }
        public int? EquipmentID { get; set; }
        public int? MerchantID { get; set; }

        public string? MerchantName { get; set; }
        public int ChannelID { get; set; }
        //public string? Status { get; set; }

        public DateTime? ResolvedDate { get; set; }

        public string? TicketNo { get; set; }
        public int? SatisfactionScore { get; set; }

        public string? OTP { get; set; }
        public int? ManagementId { get; set; }
        public string? Attachment { get; set; }
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
