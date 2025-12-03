using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchants.Core.Common;
using System.Text.Json.Serialization;

namespace Merchants.Core.Entities
{
    [Table("tblRoles")]
    public class Roles : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set;}

    }
}
