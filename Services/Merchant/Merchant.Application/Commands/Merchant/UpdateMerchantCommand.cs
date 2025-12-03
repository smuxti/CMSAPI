using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Merchant
{
    public class UpdateMerchantCommand : IRequest<Response>
    {
        public int ID { get; set; }
        public string MerchantName { get; set; }
        public string MerchantAddress { get; set; }
        public string? Email { get; set; }
        public string? OtherEmail { get; set; }
        public string? Number { get; set; }
        public string? OtherNumber { get; set; }
        public string City { get; set; }
        public int Zone { get; set; }
        public int Area { get; set; }

    }
}
