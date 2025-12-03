using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Merchants.Core.Common;

namespace Merchants.Core.Entities
{
    [Table("tblComplaintDetails")]
    public class ComplaintDetails:BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string? TickentNo { get; set; }
        public int ComplaintID { get; set; }

        public int? ManagementId { get; set; }
        public DateTime EscalationTime { get; set; }
        public int EscalationId { get; set; }
        public int Level { get; set; }
        public string CurrentStatus { get; set; }
        public string Description { get; set; }
    }

}
