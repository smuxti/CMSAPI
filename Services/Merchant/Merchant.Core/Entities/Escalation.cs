using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchants.Core.Common;

namespace Merchants.Core.Entities
{

    [Table("tblEscalation")]
    public class Escalation:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Level { get; set; }
        public int? ManagementID { get; set; }
        public int CategoryID { get; set; }
        public string? OtherEmail { get; set; }
        public string Email { get; set; }
        public string? ContactNumber { get; set; }

        public int? Type { get; set; }
        public int ResponseTime { get; set; }
        public string ResponseType { get; set; }
        //public object ManagementDetails { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public int? CreatedBy { get; set; }
        //public int? UpdateBy { get; set; }
        //public DateTime? DeleteDate { get; set; }
        //public int? DeleteBy { get; set; }
        //public bool? IsDelete { get; set; }
        //public string? Remarks { get; set; }
    }
}
