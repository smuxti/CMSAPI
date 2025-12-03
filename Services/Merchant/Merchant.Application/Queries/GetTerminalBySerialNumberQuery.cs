using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Queries
{
    public class GetTerminalBySerialNumberQuery: IRequest<Response>
    {
        public string SerialNumber { get; set; }
        public GetTerminalBySerialNumberQuery(string SerialNumber)
        {
            this.SerialNumber = SerialNumber;
        }
    }
}
