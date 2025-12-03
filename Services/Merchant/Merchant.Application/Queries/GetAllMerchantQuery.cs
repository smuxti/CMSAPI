using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class GetAllMerchantQuery : IRequest<Response>
    {
        public  GetAllMerchantQuery()
        {
        }
    }
}
