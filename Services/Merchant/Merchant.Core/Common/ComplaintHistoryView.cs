using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Common
{
    public class ComplaintHistoryView
    {
        public int ID { get; set; }
        public DateTime? ComplaintDate { get; set; }
        public string? TicketNo { get; set; }
        public string? Description { get; set; }
        public int CategoryID { get; set; }
        public string? Category { get; set; }
        public int Type { get; set; }
        public string? ComplaintTypes { get; set; }
        public int ChannelID { get; set; }
        public string? Channel { get; set; }
        public int ComplainerID { get; set; }
        public string? ComplainerName { get; set; }
        public string? ComplainerEmail { get; set; }
        public string? ComplainerCellNo { get; set; }
        public string? CurrentStatus { get; set; }
        public string? ComplDtlDescription { get; set; }
        public string? ComplDtlRemarks { get; set; }
        public int? ManagementID { get; set; }
        public int? EquipmentID { get; set; }
        public int Level { get; set; }
        public string? POCName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CompDescription { get; set; }
        public string CompRemarks { get; set; }
        public string LatestStatus { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}