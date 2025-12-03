using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Entities
{
    [Table("tblRoutes")]
    public class Routes
    {
        [Key]
        public int Id { get; set; }
        public string RouteName  { get; set; }
        public int RouteId { get; set; }
    }
}
