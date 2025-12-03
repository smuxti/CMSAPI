using Authentication.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Entities
{
    [Table("tblUserResources")]
    public class UserResource : BaseEntity
    {
        public Guid MerchantId { get; set; }
        public string FileType { get; set; }
        public string URL { get; set; }
    }
}
