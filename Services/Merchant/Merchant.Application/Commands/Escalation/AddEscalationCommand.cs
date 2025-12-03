using MediatR;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Escalation
{
    public class AddEscalationCommand:IRequest<Response>
    {
        //public int CategoryID { get; set; }
        //public int? Type { get; set; }

        public IEnumerable<EscalationList>? Escalations { get; set; }

        //public int Level { get; set; }
        //public int? ManagementID { get; set; }

        //public int CategoryID { get; set; }

        //public string Email { get; set; }
        //public string? ContactNumber { get; set; }
        //public int? Type { get; set; }
        //public int ResponseTime { get; set; }
        //public string ResponeType { get; set; }
    }

    public class EscalationList
    {
        public int CategoryID { get; set; }
        public int? Type { get; set; }
        public int Level { get; set; }
        public int? ManagementID { get; set; }
        public string? OtherEmail { get; set; }
        public string Email { get; set; }
        public string? ContactNumber { get; set; }
        public int ResponseTime { get; set; }
        public string ResponseType { get; set; }

    }
}

