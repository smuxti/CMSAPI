using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Merchant
{
    public class GetAllMerchantCommand : IRequest<Response>
    {
        //public int ID { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }
        public string MerchantAddress { get; set; }
        public string City { get; set; }

    }
}
