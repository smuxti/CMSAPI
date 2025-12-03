using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.ComplaintCategory
{
    public class UpdateComplaintCategoryCommand : IRequest<Response>
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public string Category { get; set; }
        public string? AltName { get; set; }
        //public int ResponseTime { get; set; }
        //public string ResponeType { get; set; }
        //public string? Remarks { get; set; }


    }
}
