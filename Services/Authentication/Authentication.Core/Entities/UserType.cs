using Authentication.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Entities
{
    [Table("tblUserType")]
    public class UserType: BaseEntity
    {
        public string TypeName { get; set; }
        public int TypeCode { get; set; }

    }
}
