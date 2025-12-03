using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Merchants.Application.Queries
{
    public class GetYearlyComplaintsCountQuery : IRequest<List<int>>
    {
        public string Year { get; set; }
        public GetYearlyComplaintsCountQuery() { }
    }
}
