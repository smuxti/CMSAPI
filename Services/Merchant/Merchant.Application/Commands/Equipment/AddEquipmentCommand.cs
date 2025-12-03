using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Commands.Equipment
{
    public class AddEquipmentCommand : IRequest<Response>
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }
}
