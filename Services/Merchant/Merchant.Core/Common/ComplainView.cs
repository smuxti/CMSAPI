using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Common
{
    public class ComplainView
    {
        public int ID { get; set; }
        public DateTime? ComplaintDate { get; set; }
        public string? TicketNo { get; set; }
        public int ComplainerID { get; set; }
        public string? ComplainerCell { get; set; }
        public string? Name { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public int TypeID { get; set; }
        public string? ComplaintTypes { get; set; }
        public int CategoryID { get; set; }
        public string? Category { get; set; }
        public int? MerchantID { get; set; }
        public int? EquipmentID { get; set; }
        public string? POCName { get; set; }
        public int? Mangementid { get; set; }
        public int ChannelID { get; set; }
        public string? ChannlerType { get; set; }
        //public string? Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ResolvedDate { get; set; }

        public int? SatisfactionScore { get; set; }

        public string? OTP { get; set; }
        public string? CategoryAltname { get; set; }
        public string? MerchantName { get; set; }
        public string? MerchantCity { get; set; }

    }
}
