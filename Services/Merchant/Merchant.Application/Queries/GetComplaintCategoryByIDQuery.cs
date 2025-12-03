using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class GetComplaintCategoryByIDQuery : IRequest<Response>
    {
        public int ID { get; set; }
        public GetComplaintCategoryByIDQuery(int id)
        {
            ID = id;
        }
    }
}
