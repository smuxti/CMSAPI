using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class GetMerchantLocationByIDQuery : IRequest<Response>
    {
        public int Id { get; set; }
        public GetMerchantLocationByIDQuery(int id)
        {
            this.Id = id;
        }
    }
}
