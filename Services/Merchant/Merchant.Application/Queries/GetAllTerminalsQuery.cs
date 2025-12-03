using MediatR;
using Merchants.Core.Entities;

namespace Merchants.Application.Queries
{
    public class GetAllTerminalsQuery: IRequest<IReadOnlyList<Terminal>>
    {
        public GetAllTerminalsQuery()
        {
            
        }
    }
}
