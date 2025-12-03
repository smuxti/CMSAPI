using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Queries
{
    public class GetMerchantListQuery:IRequest<List<MerchantResponse>>
    {
        public string Name { get; set; }
        public GetMerchantListQuery(string Name)
        {
            this.Name = Name;
        }
    }
}
