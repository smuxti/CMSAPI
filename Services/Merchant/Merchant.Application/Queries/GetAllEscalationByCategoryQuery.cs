using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class GetAllEscalationByCategoryQuery : IRequest<Response>
    {
        public int CategoryID { get; set; }
        public int? Type { get; set; }
        //}   
        public GetAllEscalationByCategoryQuery(int categoryID,int complaintTypeID )
        {
            CategoryID = categoryID;
            Type = complaintTypeID;
        }

    }
}
