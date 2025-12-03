using Merchants.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands
{
    public class GetUserTypesCommands : IRequest<IReadOnlyList<UserType>>
    {
        public GetUserTypesCommands()
        {
                
        }
    }
}
