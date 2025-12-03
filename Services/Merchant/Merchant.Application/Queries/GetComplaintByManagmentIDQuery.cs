using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class GetComplaintByManagmentIDQuery : IRequest<Response>
    {
        public int ManagementID { get; set; }
        public GetComplaintByManagmentIDQuery(int managementID)
        {
            ManagementID = managementID;
        }
    }
}
