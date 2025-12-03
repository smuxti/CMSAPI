using Merchants.Application.Responses;
using Merchants.Core.Entities;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class GetWeeklyComplainQuery : IRequest<Response>
    {
    }
}
