using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.ComplaintCategory
{
    public class AddComplaintCategoryCommand : IRequest<Response>
    {

        public int Type { get; set; }
        public string Category { get; set; }
        public string? AltName { get; set; }
        //public int ResponseTime { get; set; }
        //public string ResponeType { get; set; }

        //public DateTime CreatedDate { get; set; }
        //public int CreatedBy { get; set; }
    }
}
