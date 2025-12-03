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
    [Table("tblUserResources")]
    public class UserResource : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public string FileType { get; set; }
        public string URL { get; set; }
    }
}
