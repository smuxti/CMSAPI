using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Escalation
{
    public class UpdateEscalationCommand:IRequest<Response>
    {
        public int MatrixID { get; set; }
        public int Level { get; set; }
        public int CategoryID { get; set; }
        public string? OtherEmail { get; set; }
        public int? ManagementID { get; set; }
        public string Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? Remarks { get; set; }
        public int? Type { get; set; }
        public int ResponseTime { get; set; }
        public string ResponseType { get; set; }
        public bool IsDeleted { get; set; }


    }
}
