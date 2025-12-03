using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Merchants.Application.Queries
{
    public class GetComplaintStatusCountQuery : IRequest<List<int>>
    {
        public string? year { get; set; }
        public string? month { get; set; }
        public string? week { get; set; }
        public GetComplaintStatusCountQuery() { }
    }
}
