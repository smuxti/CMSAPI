using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Queries
{
    public class GetTopCategoriesQuery : IRequest<Response>
    {
        public string? year { get; set; }
        public string? month { get; set; }
        public GetTopCategoriesQuery() { }
    }
}
