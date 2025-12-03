using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Escalation
{
    public class GetEscalationByCategoryCommand
    {
        public int MatrixID { get; set; }
        public int Level { get; set; }
        public int ManagementID { get; set; }
        public string? Email { get; set; }
        public string? ContactNo { get; set; }
        public int CategoryID { get; set; }
        public string? Category { get; set; }
        public int Type { get; set; }
        public string? ComplaintType { get; set; }
        public string? ResponseType { get; set; }
        public int ResponseTime { get; set; }
        
    }
}
