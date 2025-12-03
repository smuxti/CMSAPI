using Merchants.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Entities
{
    [Table("tblComplaintCategory")]
    public class ComplaintCategory:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        //public int? Type { get; set; }
        public string Category { get; set; }
        public int Type { get; set; }
        public string? AltName { get; set; }
        //public int ResponseTime { get; set; }
        //public string ResponeType { get; set; }

        //public DateTime CreatedDate { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public int? UpdateBy { get; set; }
        //public DateTime? DeleteDate { get; set; }
        //public int? DeleteBy { get; set; }
        //public bool? IsDelete { get; set; }
        //public string Remarks { get; set; }
    }
}
