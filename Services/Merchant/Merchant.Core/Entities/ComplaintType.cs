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
    [Table("tblComplaintType")]
    public class ComplaintType:BaseEntity
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ComplaintTypes { get; set; }
    }
}
