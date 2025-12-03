using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Entities
{
    [Table("tblRoleRoutes")]
    public class RoleRouts
    {
        [Key]
        public int Id { get; set; }
        public int RoleTypeId { get; set; }
        public int RouteId { get; set; }
    }
}
