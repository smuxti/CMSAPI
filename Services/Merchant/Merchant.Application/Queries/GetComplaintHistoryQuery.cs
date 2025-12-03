using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class GetComplaintHistoryQuery : IRequest<Response>
    {
        public int ComplainID { get; set; }
        public GetComplaintHistoryQuery(int _id)
        {
            ComplainID = _id;
        }
    }
}
