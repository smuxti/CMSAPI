using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Infrastructure.Data
{
    public class EscalationView
    {
        public int ID { get; set; }
        public int Level { get; set; }
        public int? ManagementID { get; set; }
        public int CategoryID { get; set; }
        public string? Category { get; set; }
        public string Email { get; set; }
        public string? ContactNumber { get; set; }
        public int? Type { get; set; }
        public string? ComplaintTypes { get; set; }
        public int ResponseTime { get; set; }
        public string ResponseType { get; set; }

    }
}
