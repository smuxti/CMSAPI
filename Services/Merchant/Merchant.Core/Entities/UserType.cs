using Merchants.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Entities
{
    [Table("tblUserType")]
    public class UserType: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string TypeName { get; set; }
        public int TypeCode { get; set; }
        public bool? Locked { get; set; }
    }
}
