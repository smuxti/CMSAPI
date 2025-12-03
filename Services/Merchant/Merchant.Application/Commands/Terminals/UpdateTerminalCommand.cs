using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Terminals
{
    public class UpdateTerminalCommand : AddTerminalCommand ,IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
