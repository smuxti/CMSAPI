using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Commands.Merchants
{
    public class MerchantPostCommand: IRequest<Response>
    {
        public string MerchantCode { get; set; }
        public string ReasonCode { get; set; }

    }
}
