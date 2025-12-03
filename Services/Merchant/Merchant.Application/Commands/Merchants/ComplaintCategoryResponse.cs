using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Merchants
{
    public class ComplaintCategoryResponse: IRequest<Response>
    {
        public int? ComplaintTypeID { get; set; }
        public string Category { get; set; }
        public int ResponseTime { get; set; }
        public string ResponeType { get; set; }
    }
}
